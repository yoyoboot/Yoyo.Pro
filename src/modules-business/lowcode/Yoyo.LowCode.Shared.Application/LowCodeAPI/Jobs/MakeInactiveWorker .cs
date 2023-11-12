// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Abp.Dependency;
using Abp.Domain.Repositories;
using Abp.Domain.Uow;
using Abp.Runtime.Caching;
using Abp.Threading.BackgroundWorkers;
using Abp.Threading.Timers;
using Masuit.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using MySql.Data.MySqlClient;
using Oracle.ManagedDataAccess.Client;
using SqlSugar;
using Yoyo.LowCode.LowCodeAPI.DomainService.SqlService;
using Yoyo.LowCode.LowCodeAPI.Dtos.CommonDto;

namespace Yoyo.LowCode.LowCodeAPI.Jobs
{
    public class MakeInactiveWorker : PeriodicBackgroundWorkerBase, ISingletonDependency
    {
        private readonly IRepository<Apply, Guid> _applyRepository;
        private readonly IRepository<ApplyHealthTesting, Guid> _applyHealthRepository;

        //缓存配置
        private readonly ICacheManager _cacheManager;

        //数据库帮助类
        public ISqlHelperManager _sqlHelper;

        //host
        private readonly IWebHostEnvironment _environment;

        public MakeInactiveWorker(AbpTimer timer, IRepository<Apply, Guid> apply,
            IRepository<ApplyHealthTesting, Guid> applyHealth,
            ICacheManager cacheManager,
            ISqlHelperManager helperManager,
            IWebHostEnvironment hostEnvironment)
       : base(timer)
        {
            _applyRepository = apply;
            _applyHealthRepository = applyHealth;
            Timer.Period = 5000; //5 seconds (good for tests, but normally will be more)
            _cacheManager = cacheManager;
            _sqlHelper = helperManager;
            _environment = hostEnvironment;
        }

        [UnitOfWork]
        protected override async void DoWork()
        {
            try
            {
                //连接器 链接健康检测
                await ApplyHealthTesting_ON();
            }
            catch (Exception)
            {
                throw;
            }

            //using (CurrentUnitOfWork.DisableFilter(AbpDataFilters.MayHaveTenant))
            //{
            //    var oneMonthAgo = Clock.Now.Subtract(TimeSpan.FromDays(30));

            //    CurrentUnitOfWork.SaveChanges();
            //}
        }

        //链接健康检测
        protected async Task<bool> ApplyHealthTesting_ON()
        {
            var applyList = _applyRepository.GetAllList();
            var health = _applyHealthRepository.GetAllList()
                .Where(t => applyList.Select(m => m.Id).Contains(t.ApplyID));
            //.Where(t => t.HealthTestingModeName == "HTTP URL模式");

            foreach (var item in health)
            {
                TestingApplyHealthDto testingApplyHealth = new TestingApplyHealthDto();
                testingApplyHealth.ApplyID = item.ApplyID;
                int detectionInterval = TimeConversion(item.DetectionInterval, item.DITypeName);
                //根据时间间隔去检测
                if (CheckApplyHealthTesting_Cache(item.ApplyID, detectionInterval))
                {
                    var apply = applyList.Where(t => t.Id == item.ApplyID).FirstOrDefault();
                    if ((apply.ApplyTypeName.ToLower() == "http".ToLower()
                        || apply.ApplyTypeName.ToLower() == "webservice".ToLower())
                        && item.HealthTestingModeName == "HTTP URL模式")
                    {
                        try
                        {
                            // 发送HTTP请求并获取响应
                            using (var client = new HttpClient())
                            {
                                //设置检测 连接超时时间
                                client.Timeout = TimeSpan.FromSeconds(TimeConversion(item.TimeOut, item.TOTypeName));

                                //开始请求
                                var response = await client.GetAsync(item.CheckUrl);
                                var statusCode = response.StatusCode;

                                // 判断响应状态码
                                if (response.IsSuccessStatusCode)
                                {
                                    testingApplyHealth.IsConnection = true;
                                    testingApplyHealth.StatusCode = 200;
                                    testingApplyHealth.Message = "成功";
                                    SetApplyHealthTesting_Cache(item.ApplyID, detectionInterval, testingApplyHealth);
                                }
                                else
                                {
                                    testingApplyHealth.IsConnection = false;
                                    testingApplyHealth.StatusCode = ((int)statusCode);
                                    testingApplyHealth.Message = response.RequestMessage.ToString();
                                    SetApplyHealthTesting_Cache(item.ApplyID, detectionInterval, testingApplyHealth);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            testingApplyHealth.IsConnection = false;
                            testingApplyHealth.StatusCode = 505;
                            testingApplyHealth.Message = ex.Message.ToString();
                            SetApplyHealthTesting_Cache(item.ApplyID, detectionInterval, testingApplyHealth);
                        }
                    }
                    else if (apply.ApplyTypeName.ToLower() == "sqlserver".ToLower()
                        || apply.ApplyTypeName.ToLower() == "mysql".ToLower()
                        || apply.ApplyTypeName.ToLower() == "oracle".ToLower())
                    {
                        //参数
                        string sqlType = apply.SqlType;
                        string SqlHost = apply.SqlHostTest;
                        int SqlPort = apply.SqlPortTest;
                        string sqlDatabase = apply.SqlDBTest;
                        string sqlUserName = apply.SqlUserNameTest;
                        string sqlPassWord = apply.SqlPassWordTest;
                        if (_environment.IsProduction())
                        {
                            SqlHost = apply.SqlHostProduce;
                            SqlPort = apply.SqlPortProduce;
                            sqlDatabase = apply.SqlDBProduce;
                            sqlUserName = apply.SqlUserNameProduce;
                            sqlPassWord = apply.SqlPassWordProduce;
                        }

                        var sqlserverConn = _sqlHelper.ToConnectionString(apply.ApplyTypeName.ToLower(), SqlHost, SqlPort, sqlUserName, sqlPassWord, sqlDatabase);
                        try
                        {
                            if (apply.ApplyTypeName.ToLower() == "sqlserver".ToLower())
                            {
                                using (SqlConnection connection = new SqlConnection(sqlserverConn))
                                {
                                    connection.Open();

                                    // 执行简单的 SQL 查询来检测数据库的健康状态
                                    string sql = "SELECT 1";
                                    using (SqlCommand command = new SqlCommand(sql, connection))
                                    {
                                        command.ExecuteScalar();
                                    }

                                    testingApplyHealth.IsConnection = true;
                                    testingApplyHealth.StatusCode = 200;
                                    testingApplyHealth.Message = "成功";
                                    SetApplyHealthTesting_Cache(item.ApplyID, detectionInterval, testingApplyHealth);
                                }
                            }
                            else if (apply.ApplyTypeName.ToLower() == "mysql".ToLower())
                            {
                                using (MySqlConnection connection = new MySqlConnection(sqlserverConn))
                                {
                                    connection.Open();

                                    // 执行简单的 SQL 查询来检测数据库的健康状态
                                    string sql = "SELECT 1";
                                    using (MySqlCommand command = new MySqlCommand(sql, connection))
                                    {
                                        command.ExecuteScalar();
                                    }

                                    testingApplyHealth.IsConnection = true;
                                    testingApplyHealth.StatusCode = 200;
                                    testingApplyHealth.Message = "成功";
                                    SetApplyHealthTesting_Cache(item.ApplyID, detectionInterval, testingApplyHealth);
                                }
                            }
                            else if (apply.ApplyTypeName.ToLower() == "oracle".ToLower())
                            {
                                using (OracleConnection connection = new OracleConnection(sqlserverConn))
                                {
                                    connection.Open();

                                    // 执行简单的 SQL 查询来检测数据库的健康状态
                                    string sql = "SELECT 1 FROM DUAL";
                                    using (OracleCommand command = new OracleCommand(sql, connection))
                                    {
                                        command.ExecuteScalar();
                                    }

                                    testingApplyHealth.IsConnection = true;
                                    testingApplyHealth.StatusCode = 200;
                                    testingApplyHealth.Message = "成功";
                                    SetApplyHealthTesting_Cache(item.ApplyID, detectionInterval, testingApplyHealth);
                                }
                            }
                        }
                        catch (Exception ex)
                        {
                            // 检查异常信息，可以根据需要进行自定义的错误处理逻辑
                            // 例如，连接超时、认证失败等异常可能会导致健康检测失败
                            testingApplyHealth.IsConnection = false;
                            testingApplyHealth.StatusCode = 505;
                            testingApplyHealth.Message = ex.Message.ToString();
                            SetApplyHealthTesting_Cache(item.ApplyID, detectionInterval, testingApplyHealth);
                        }
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 根据间隔时间 检测(key存在未过期 就不检测)
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="detectionInterval">秒</param>
        protected bool CheckApplyHealthTesting_Cache(Guid guid, int detectionInterval)
        {
            string heathKey = "ApplyHealthTesting-" + guid.ToString();
            TestingApplyHealthDto testingApplyHealthDto = new TestingApplyHealthDto();
            testingApplyHealthDto.IsConnection = true;

            //找到缓存中的 连接短日志
            ITypedCache<string, CacheItemDto> API_FC_Cache = _cacheManager.GetCache(heathKey).AsTyped<string, CacheItemDto>();
            var msg = API_FC_Cache.TryGetValue(heathKey, out CacheItemDto cacheItem);
            if (msg == false)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// 设置 检测结果
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="detectionInterval"></param>
        /// <returns></returns>
        protected bool SetApplyHealthTesting_Cache(Guid guid, int detectionInterval, TestingApplyHealthDto testingApplyHealthDto)
        {
            string heathKey = "ApplyHealthTesting-" + guid.ToString();

            List<TestingApplyHealthDto> healthDtos = new List<TestingApplyHealthDto>();
            healthDtos.Add(testingApplyHealthDto);
            //找到缓存中的异常记录
            ITypedCache<string, CacheItemDto> API_FC_Cache = _cacheManager.GetCache(heathKey).AsTyped<string, CacheItemDto>();
            //设置过期时间（检测时间间隔）
            API_FC_Cache.DefaultSlidingExpireTime = TimeSpan.FromSeconds(detectionInterval);

            CacheItemDto cacheItemDto = new CacheItemDto();
            cacheItemDto.Key = guid.ToString();
            cacheItemDto.testingApplyHealths = healthDtos;

            API_FC_Cache.Set(heathKey, cacheItemDto);
            return true;
        }

        /// <summary>
        /// 时间转换 最终为单位秒  如果类型不对直接返回0
        /// </summary>
        /// <returns></returns>
        private int TimeConversion(int unitTime, string type)
        {
            switch (type)
            {
                case "秒":
                    return unitTime;

                case "分钟":
                    return unitTime * 60;

                case "小时":
                    return unitTime * 3600;

                case "天":
                    return unitTime * 86400;

                default:
                    return 0;
            }
        }
    }
}
