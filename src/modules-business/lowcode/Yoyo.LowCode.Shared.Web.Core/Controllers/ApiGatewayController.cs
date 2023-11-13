// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Web;
using System.Xml;
using Abp.Application.Services.Dto;
using Abp.Runtime.Caching;
using Masuit.Tools;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Polly;
using SqlSugar;
using Yoyo.LowCode.LowCodeAPI;
using Yoyo.LowCode.LowCodeAPI.DomainService;
using Yoyo.LowCode.LowCodeAPI.DomainService.SqlService;
using Yoyo.LowCode.LowCodeAPI.Dtos.AgentAPIDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyApiParameterDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ApplyHealthTestingDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.CommonDto;
using Yoyo.LowCode.LowCodeAPI.Dtos.ReponseDataDto;

namespace Yoyo.LowCode.Controllers
{
    [ApiController]
    //[Route("[controller]/[action]")]
    [Route("[controller]")]
    public class ApiGatewayController : LowCodeSharedControllerBase
    {
        private readonly HttpClient _httpClient;
        private static readonly CookieContainer _cookieContainer = new CookieContainer();

        //host
        private readonly IWebHostEnvironment _environment;

        //缓存配置
        private readonly ICacheManager _cacheManager;

        //连接应用
        public IApplyAppService _applyApp;

        //链接应用健康检测信息
        public IApplyHealthTestingAppService _applyHealthApp;

        //连接应用API
        public IApplyAPIAppService _applyAPIApp;

        //连接应用API 授权信息
        public IApplyAuthenticationAppService _applyAuthenticationApp;

        //接口中心API
        public IAgentAPIAppService _agentAPIApp;

        //query  header参数配置表
        public IApplyApiParameterAppService _applyApiParameterApp;

        //数据库查询
        public ISqlHelperManager _sqlHelper;

        //接口请求记录
        public IAPIRequestRecordManager _apiLog;

        //授权应用API
        public IAppAuthAppService _appAuthApp;

        //流量控制appservice
        public IFlowControlAppService _flowControlApp;

        //webservice服务
        public IWebServiceAppService _webServiceAppService;

        public ApiGatewayController(
            IWebHostEnvironment hostEnvironment,
            IHttpClientFactory httpClientFactory,
            ICacheManager cacheManager,
            IApplyAppService applyApp,
            IApplyHealthTestingAppService applyHealthApp,
            IApplyAPIAppService applyAPIApp,
            IAgentAPIAppService agentAPIApp,
            IApplyAuthenticationAppService applyAuthenticationApp,
            IApplyApiParameterAppService applyApiParameterApp,
            ISqlHelperManager sqlHelper,
            IAPIRequestRecordManager apiLog,
            IAppAuthAppService appAuthApp,
            IFlowControlAppService flowControlApp,
            IWebServiceAppService webServiceAppService)
        {
            _httpClient = httpClientFactory.CreateClient();
            _environment = hostEnvironment;
            _cacheManager = cacheManager;
            _applyApp = applyApp;
            _applyHealthApp = applyHealthApp;
            _applyAPIApp = applyAPIApp;
            _agentAPIApp = agentAPIApp;
            _applyAuthenticationApp = applyAuthenticationApp;
            _applyApiParameterApp = applyApiParameterApp;
            _sqlHelper = sqlHelper;
            _apiLog = apiLog;
            _appAuthApp = appAuthApp;
            _flowControlApp = flowControlApp;
            _webServiceAppService = webServiceAppService;
        }

        [HttpGet("{api}/{*url}")]
        [HttpPost("{api}/{*url}")]
        public async Task<IActionResult> ProxyRequest(string api, string url)
        {
            Guid stepBatch = Guid.NewGuid();
            APIRequestRecord step1 = new APIRequestRecord();
            string StrrequestBody = string.Empty;

            #region 日志记录1  网关接收请求

            step1.Id = Guid.NewGuid();
            step1.StepBatch = stepBatch;
            step1.StepName = "网关接收请求";
            step1.StepSort = 10;
            step1.StepType = "接收请求";
            step1.StartTime = DateTime.Now;
            step1.RequestSourceIP = HttpContext.Connection.RemoteIpAddress.ToString(); ;
            step1.RequestURL = HttpContext.Request.Scheme + "://" + HttpContext.Request.Host + HttpContext.Request.Path;
            step1.RequestMethod = HttpContext.Request.Method;
            var queryString = HttpContext.Request.Query
            .Select(q => $"{q.Key}={q.Value}")
            .ToJsonString();
            step1.RequestQuerys = string.Join("\n", queryString);

            var headerString = HttpContext.Request.Headers
            .Select(h => $"{h.Key}: {h.Value}")
            .ToJsonString();
            step1.RequestHeaders = string.Join("\n", headerString);

            #endregion 日志记录1  网关接收请求

            using (StreamReader reader = new StreamReader(HttpContext.Request.Body, Encoding.UTF8))
            {
                StrrequestBody = reader.ReadToEndAsync().Result.ToString();
                if (string.IsNullOrWhiteSpace(StrrequestBody))
                {
                    StrrequestBody = "{}";
                }
                StrrequestBody = StrrequestBody.Replace("\\r", "").Replace("\\n", "").Replace("\\", "");
                step1.RequestBody = StrrequestBody;
            }
            ReponseData data = new ReponseData();
            data.stepBatch = stepBatch;
            //前端要求数据返回格式类型
            if (HttpContext.Request.Headers.ContainsKey("YoyoResponseDataType"))
            {
                int.TryParse(HttpContext.Request.Headers["YoyoResponseDataType"], out int responseDataType);
                data.responseDataType = responseDataType;
            }
            //调试需要的GTCstepBatch
            if (HttpContext.Request.Headers.ContainsKey("GTCstepBatch"))
            {
                Guid.TryParse(HttpContext.Request.Headers["GTCstepBatch"], out Guid GTCstepBatch);
                if (GTCstepBatch != Guid.Empty)
                {
                    stepBatch = GTCstepBatch;
                    data.stepBatch = stepBatch;
                    step1.StepBatch = GTCstepBatch;
                }
            }

            //准备数据
            var queryParams = HttpContext.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
            var headerParams = HttpContext.Request.Query.ToDictionary(q => q.Key, q => q.Value.ToString());
            //判断 校验请求路径是否存在
            string apiUrl = HttpContext.Request.Path.ToString().Replace("/ApiGateway", "");
            var agentAPI = await _agentAPIApp.GetAgentAPIByPath(apiUrl);
            if (agentAPI == null)
            {
                #region 日志记录1  网关接收请求（请求路径不存在）

                step1.AgentAPIID = Guid.Empty;
                step1.EndTime = DateTime.Now;
                step1.StepStatus = false;
                step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                step1.ReponseCode = "400";
                step1.ReponseMessage = $"当前请求的API不存在，请检查API配置的Path路径{apiUrl}";
                await ApiSetLog(step1);

                #endregion 日志记录1  网关接收请求（请求路径不存在）

                data.success = false;
                data.code = "404";
                data.message = "当前请求的API不存在，请检查API配置的Path路径";
                data.exception = null;
                data.data = null;
                return ApiResponse(data, agentAPI);
            }
            else
            {
                #region 日志记录1  网关接收请求 步骤一记录完成

                step1.AgentAPIID = agentAPI.Id;
                step1.EndTime = DateTime.Now;
                step1.StepStatus = true;
                step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                await ApiSetLog(step1);

                #endregion 日志记录1  网关接收请求 步骤一记录完成
            }

            #region 准备一些数据

            //链接应用 信息
            var apply = await _applyApp.GetByCode(agentAPI.ApplyCode);
            EntityDto<Guid> apply_entityDto = new EntityDto<Guid>();

            //链接应用 健康检测数据
            ApplyHealthTestingListDto applyHealthTesting = new ApplyHealthTestingListDto();
            if (apply != null)
            {
                apply_entityDto.Id = apply.Id;
                applyHealthTesting = await _applyHealthApp.GetByApplyID(apply_entityDto);
            }

            #endregion 准备一些数据

            #region 日志记录2 参数校验 开始

            step1.Id = Guid.NewGuid();
            step1.StepName = "参数校验";
            step1.StepSort = 20;
            step1.StepType = "参数校验";
            step1.StartTime = DateTime.Now;

            #endregion 日志记录2 参数校验 开始

            //流量控制策略 校验
            if (!await CheckFlowControl(agentAPI))
            {
                #region 日志记录2 单位时间内流量限制

                step1.EndTime = DateTime.Now;
                step1.StepStatus = false;
                step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                step1.ReponseCode = "409";
                step1.ReponseMessage = $"当前请求的API已经超过单位时间流量限制";
                await ApiSetLog(step1);

                #endregion 日志记录2 单位时间内流量限制

                data.success = false;
                data.code = "409";
                data.message = $"当前请求的API已经超过单位时间流量限制";
                data.exception = null;
                data.data = null;
                return ApiResponse(data, agentAPI);
            }

            //健康检测  熔断策略校验
            if (!CheckFusingModel(agentAPI, applyHealthTesting))
            {
                #region 日志记录2 参数校验

                step1.EndTime = DateTime.Now;
                step1.StepStatus = false;
                step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                step1.ReponseCode = "400";
                step1.ReponseMessage = $"已熔断，达到熔断策略限制！";
                await ApiSetLog(step1);

                #endregion 日志记录2 参数校验

                data.success = false;
                data.code = "400";
                data.message = "已熔断，达到熔断策略限制！";
                data.exception = null;
                data.data = null;
                return ApiResponse(data, agentAPI);
            };

            //API 状态是否发布
            if (!agentAPI.EnableStatus)
            {
                #region 日志记录2 参数校验

                step1.EndTime = DateTime.Now;
                step1.StepStatus = false;
                step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                step1.ReponseCode = "400";
                step1.ReponseMessage = $"当前请求的API状态未发布";
                await ApiSetLog(step1);

                #endregion 日志记录2 参数校验

                data.success = false;
                data.code = "400";
                data.message = "当前请求的API状态未发布";
                data.exception = null;
                data.data = null;
                return ApiResponse(data, agentAPI);
            }

            //判断 请求方式 和 接口中心预设配置是否相符
            string Method = HttpContext.Request.Method.ToUpper();
            if (Method != agentAPI.MethodCode.ToUpper())
            {
                #region 日志记录2 参数校验

                step1.EndTime = DateTime.Now;
                step1.StepStatus = false;
                step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                step1.ReponseCode = "400";
                step1.ReponseMessage = $"当前API不支持{Method}请求";
                await ApiSetLog(step1);

                #endregion 日志记录2 参数校验

                data.success = false;
                data.code = "400";
                data.message = $"当前API不支持{Method}请求";
                data.exception = null;
                data.data = null;
                return ApiResponse(data, agentAPI);
            }

            //request必须符合json格式 (要改成根据设置里面的)

            #region request必须符合json格式

            //JsonDocument doc = JsonDocument.Parse(StrrequestBody);
            //if (doc.RootElement.ValueKind != JsonValueKind.Object)
            //{
            //    #region 日志记录2 参数校验
            //    step1.EndTime = DateTime.Now;
            //    step1.StepStatus = false;
            //    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

            //    step1.ReponseCode = "400";
            //    step1.ReponseMessage = $"RequestBody传参格式不正确";
            //    await ApiSetLog(step1);
            //    #endregion 日志记录2 参数校验
            //    return ApiResponse(data, agentAPI);
            //}

            #endregion request必须符合json格式

            //判断 接口是否启用安全认证，启用了安全认证就需要查看 是否带有授权Key以及授权key是否有权限
            if (agentAPI.IsSafetySertification)
            {
                //是否带有授权应用Key Token
                var IsKey = HttpContext.Request.Headers.ContainsKey("YoyoApiGatewayToken");
                if (!IsKey)
                {
                    #region 日志记录2 参数校验

                    step1.EndTime = DateTime.Now;
                    step1.StepStatus = false;
                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                    step1.ReponseCode = "400";
                    step1.ReponseMessage = $"访问此接口需要安全验证，请您携带身份验证信息重试";
                    await ApiSetLog(step1);

                    #endregion 日志记录2 参数校验

                    data.success = false;
                    data.code = "400";
                    data.message = $"访问此接口需要安全验证，请您携带身份验证信息重试";
                    data.exception = null;
                    data.data = null;
                    return ApiResponse(data, agentAPI);
                }
                else
                {
                    string appToken = HttpContext.Request.Headers["YoyoApiGatewayToken"];
                    var isauth = await _appAuthApp.CheckAppAuthYoyoApiGatewayToken(appToken, agentAPI.Id);
                    //校验授权Token是否合法
                    if (isauth == 1)
                    {
                        #region 日志记录2 参数校验

                        step1.EndTime = DateTime.Now;
                        step1.StepStatus = false;
                        step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                        step1.ReponseCode = "400";
                        step1.ReponseMessage = $"您的身份验证信息不合法，请检查后重试";
                        await ApiSetLog(step1);

                        #endregion 日志记录2 参数校验

                        data.success = false;
                        data.code = "400";
                        data.message = $"您的身份验证信息不合法，请检查后重试";
                        data.exception = null;
                        data.data = null;
                        return ApiResponse(data, agentAPI);
                    }
                    else if (isauth == 2)
                    {
                        //校验此应用key是否包含该接口授权

                        #region 日志记录2 参数校验

                        step1.EndTime = DateTime.Now;
                        step1.StepStatus = false;
                        step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                        step1.ReponseCode = "400";
                        step1.ReponseMessage = $"您还没有获得该接口的授权，请申请授权后重试";
                        await ApiSetLog(step1);

                        #endregion 日志记录2 参数校验

                        data.success = false;
                        data.code = "400";
                        data.message = $"您还没有获得该接口的授权，请申请授权后重试";
                        data.exception = null;
                        data.data = null;
                        return ApiResponse(data, agentAPI);
                    }
                }
            }

            EntityDto<Guid> entityDto = new EntityDto<Guid>();
            entityDto.Id = agentAPI.Id;
            //校验query中的参数
            var querylist = await _applyApiParameterApp.GetApiQueryData(entityDto);
            foreach (var item in querylist)
            {
                //校验Query字段必填
                if (item.IsRequired && !HttpContext.Request.Query.ContainsKey(item.FileName))
                {
                    #region 日志记录2 参数校验

                    step1.EndTime = DateTime.Now;
                    step1.StepStatus = false;
                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                    step1.ReponseCode = "400";
                    step1.ReponseMessage = $"Query参数{item.FileName}必填";
                    await ApiSetLog(step1);

                    #endregion 日志记录2 参数校验

                    data.success = false;
                    data.code = "400";
                    data.message = $"Query参数{item.FileName}必填";
                    data.exception = null;
                    data.data = null;
                    return ApiResponse(data, agentAPI);
                }
                else
                {
                    //校验Query字段类型 和 值是否相符
                    var itemVal = HttpContext.Request.Query[item.FileName].ToString();
                    if (!CheckParameterType(item.FileType, itemVal))
                    {
                        #region 日志记录2 参数校验

                        step1.EndTime = DateTime.Now;
                        step1.StepStatus = false;
                        step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                        step1.ReponseCode = "400";
                        step1.ReponseMessage = $"Query参数{item.FileName}字段的值必须是{item.FileType}类型";
                        await ApiSetLog(step1);

                        #endregion 日志记录2 参数校验

                        data.success = false;
                        data.code = "400";
                        data.message = $"Query参数{item.FileName}字段的值必须是{item.FileType}类型";
                        data.exception = null;
                        data.data = null;
                        return ApiResponse(data, agentAPI);
                    }
                    //校验通过直接将值保存下来
                    item.ReceivedValue = itemVal;
                }
            }

            //校验Header中的参数
            var headerlist = await _applyApiParameterApp.GetApiHeaderData(entityDto);
            foreach (var item in headerlist)
            {
                var aaa = HttpContext.Request.Headers.ContainsKey(item.FileName);
                //校验Header字段必填
                if (item.IsRequired && !HttpContext.Request.Headers.ContainsKey(item.FileName))
                {
                    #region 日志记录2 参数校验

                    step1.EndTime = DateTime.Now;
                    step1.StepStatus = false;
                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                    step1.ReponseCode = "400";
                    step1.ReponseMessage = $"Header参数{item.FileName}必填";
                    await ApiSetLog(step1);

                    #endregion 日志记录2 参数校验

                    data.success = false;
                    data.code = "400";
                    data.message = $"Header参数{item.FileName}必填";
                    data.exception = null;
                    data.data = null;
                    return ApiResponse(data, agentAPI);
                }
                else
                {
                    //校验Header字段类型 和 值是否相符
                    var itemVal = HttpContext.Request.Headers[item.FileName].ToString();
                    if (!CheckParameterType(item.FileType, itemVal))
                    {
                        #region 日志记录2 参数校验

                        step1.EndTime = DateTime.Now;
                        step1.StepStatus = false;
                        step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                        step1.ReponseCode = "400";
                        step1.ReponseMessage = $"Header参数{item.FileName}字段的值必须是{item.FileType}类型";
                        await ApiSetLog(step1);

                        #endregion 日志记录2 参数校验

                        data.success = false;
                        data.code = "400";
                        data.message = $"Header参数{item.FileName}字段的值必须是{item.FileType}类型";
                        data.exception = null;
                        data.data = null;
                        return ApiResponse(data, agentAPI);
                    }
                    //校验通过直接将值保存下来
                    item.ReceivedValue = itemVal;
                }
            }
            //校验Body中的参数
            //1 获取Body中的参数  并且 转换成 Name  Path
            List<JsonPathDto> requestPath = new List<JsonPathDto>();

            requestPath = GetJsonPath(StrrequestBody);

            //2 获取配置中的Body入参规则 并且拿到父子级路径
            var requestbodylist = await _applyApiParameterApp.GetApiRequestBodyData(entityDto, false);
            var ruleBodyPaths = from item in requestbodylist
                                let path = GetListPath(item, requestbodylist)
                                select new { item, path };
            //List<JsonPathDto> ruleBodyList = new List<JsonPathDto>();
            //3 校验前端传值  是否符合 配置规则
            foreach (var p in ruleBodyPaths)
            {
                string thPath = string.Join(".", p.path.Select(i => i.FileName));
                var requestBodyItem = requestPath.Where(t => t.Name == p.item.FileName && t.Path == thPath);
                //前置校验是否必填 如果没有传这个必填对象
                //前置校验 复制结构下 数组套数组 对象套数组等  是否必填的数量 满足 路径
                if (p.item.IsRequired)
                {
                    var parentobj = requestbodylist.Where(t => t.Id == p.item.ParentID).FirstOrDefault();
                    var parentItem = requestPath.Where(t => t.Name == parentobj.FileName).FirstOrDefault();
                    //这里计算 这个 必填字段应该有多少个
                    int parentarray = GetParentArrayCount(requestPath, thPath, 1);
                    //如果没有传这个必填对象
                    if (parentarray != requestBodyItem.Count())
                    {
                        #region 日志记录2 参数校验

                        step1.EndTime = DateTime.Now;
                        step1.StepStatus = false;
                        step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                        step1.ReponseCode = "400";
                        step1.ReponseMessage = $"Body中参数校验不通过，{thPath}下的{p.item.FileName}字段的值必填，请检查传参格式！";
                        await ApiSetLog(step1);

                        #endregion 日志记录2 参数校验

                        data.success = false;
                        data.code = "400";
                        data.message = $"Body中参数校验不通过，{thPath}下的{p.item.FileName}字段的值必填，请检查传参格式！";
                        data.exception = null;
                        data.data = null;
                        return ApiResponse(data, agentAPI);
                    }
                }

                foreach (var item in requestBodyItem)
                {
                    //3.1是否必填 校验是否包含这个值  如果包含校验格式是否正确
                    if (!CheckParameterType(p.item.FileType, item.Value))
                    {
                        #region 日志记录2 参数校验

                        step1.EndTime = DateTime.Now;
                        step1.StepStatus = false;
                        step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                        step1.ReponseCode = "400";
                        step1.ReponseMessage = $"Body中参数校验不通过,{thPath}下的{p.item.FileName}字段的值必须是{p.item.FileType}类型";
                        await ApiSetLog(step1);

                        #endregion 日志记录2 参数校验

                        data.success = false;
                        data.code = "400";
                        data.message = $"Body中参数校验不通过,{thPath}下的{p.item.FileName}字段的值必须是{p.item.FileType}类型";
                        data.exception = null;
                        data.data = null;
                        return ApiResponse(data, agentAPI);
                    }
                    //直接将值保存下来
                    p.item.ReceivedValue = item != null ? item.Value : null;
                }

                //JsonPathDto jsonPath = new JsonPathDto();
                //jsonPath.Name = p.item.FileName;
                //jsonPath.Value = p.item.DefaultValue;
                //jsonPath.Path = string.Join(".", p.path.Select(i => i.FileName));
                //ruleBodyList.Add(jsonPath);
            }

            #region 日志记录2 参数校验 结束

            step1.EndTime = DateTime.Now;
            step1.StepStatus = true;
            step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

            step1.ReponseCode = "200";
            step1.ReponseMessage = $"";
            await ApiSetLog(step1);

            #endregion 日志记录2 参数校验 结束

            //校验通过之后判断 连接器是Mock
            if (agentAPI.ServiceName.ToLower() == "Mock".ToLower())
            {
                data.success = true;
                data.code = "200";
                data.message = $"请求成功";
                data.exception = null;
                data.data = JsonConvert.DeserializeObject(agentAPI.MockData);

                #region 日志记录4 mock返回 参数校验 通过

                step1.Id = Guid.NewGuid();
                step1.StepName = "请求后台服务";
                step1.StepSort = 40;
                step1.StepType = "请求后台服务";
                step1.StartTime = DateTime.Now;
                step1.RequestSourceIP = null;
                step1.RequestURL = "Mock";
                step1.RequestMethod = "Mock";
                step1.RequestQuerys = null;
                step1.RequestHeaders = null;
                step1.RequestBody = null;
                step1.EndTime = DateTime.Now;
                step1.StepStatus = true;
                step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();
                step1.ReponseCode = "200";
                step1.ReponseMessage = $"";
                step1.ReponseHeaders = null;
                step1.ResponseBody = JsonConvert.SerializeObject(data);
                await ApiSetLog(step1);

                #endregion 日志记录4 mock返回 参数校验 通过

                return ApiResponse(data, agentAPI);
            }
            //校验通过之后判断 连接器是Http
            if (agentAPI.ServiceName.ToLower() == "Http".ToLower())
            {
                #region 日志记录3 参数转换 开始

                step1.Id = Guid.NewGuid();
                step1.StepName = "参数转换";
                step1.StepSort = 30;
                step1.StepType = "参数转换";
                step1.StartTime = DateTime.Now;

                #endregion 日志记录3 参数转换 开始

                ///映射的API信息
                var applyAPI = await _applyAPIApp.GetById(new EntityDto<Guid> { Id = agentAPI.ApplyAPIID });

                //应用信息 和 应用API信息都正常
                if (applyAPI != null && apply != null)
                {
                    //应用请求地址(根据 环境切换对应环境)
                    StringBuilder cli_httpUrl = new StringBuilder();
                    if (_environment.IsProduction())
                    {
                        cli_httpUrl.Append(apply.HttpUrlProduce);
                    }
                    else
                    {
                        cli_httpUrl.Append(apply.HttpUrlTest);
                    }
                    cli_httpUrl.Append(applyAPI.Path);

                    //http请求入参对象
                    HttpParamDto httpParam = new HttpParamDto();
                    httpParam.applyHealth = applyHealthTesting;
                    httpParam.Reponsedata = data;
                    httpParam.ApplyID = apply.Id;
                    httpParam.agentAPI = agentAPI;

                    if (agentAPI.ParameterModeName == "入参透传")
                    {
                        //query传参
                        var cli_querylist = await _applyApiParameterApp.GetApiQueryData(new EntityDto<Guid> { Id = applyAPI.Id });
                        foreach (var item in cli_querylist)
                        {
                            //是否必填 校验到底传没有传值
                            if (item.IsRequired && !HttpContext.Request.Query.ContainsKey(item.FileName))
                            {
                                #region 日志记录3 参数转换

                                step1.EndTime = DateTime.Now;
                                step1.StepStatus = false;
                                step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                step1.ReponseCode = "400";
                                step1.ReponseMessage = $"Cli_Query参数{item.FileName}必填";
                                await ApiSetLog(step1);

                                #endregion 日志记录3 参数转换

                                data.success = false;
                                data.code = "400";
                                data.message = $"连接中心_Query参数{item.FileName}必填";
                                data.exception = null;
                                data.data = null;
                                return ApiResponse(data, agentAPI);
                            }
                            //校验Cli_Query字段类型 和 值是否相符
                            var itemVal = HttpContext.Request.Query[item.FileName].ToString();
                            if (!CheckParameterType(item.FileType, itemVal))
                            {
                                #region 日志记录3 参数转换

                                step1.EndTime = DateTime.Now;
                                step1.StepStatus = false;
                                step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                step1.ReponseCode = "400";
                                step1.ReponseMessage = $"Cli_Query参数{item.FileName}字段的值必须是{item.FileType}类型";
                                await ApiSetLog(step1);

                                #endregion 日志记录3 参数转换

                                data.success = false;
                                data.code = "400";
                                data.message = $"连接中心_Query参数{item.FileName}字段的值必须是{item.FileType}类型";
                                data.exception = null;
                                data.data = null;
                                return ApiResponse(data, agentAPI);
                            }
                        }

                        ///header传参
                        Dictionary<string, string> cli_headerData = new Dictionary<string, string>();
                        var cli_headerlist = await _applyApiParameterApp.GetApiHeaderData(new EntityDto<Guid> { Id = applyAPI.Id });
                        foreach (var item in headerlist)
                        {
                            //校验Header字段必填
                            if (item.IsRequired && !HttpContext.Request.Headers.ContainsKey(item.FileName))
                            {
                                #region 日志记录3 参数转换

                                step1.EndTime = DateTime.Now;
                                step1.StepStatus = false;
                                step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                step1.ReponseCode = "400";
                                step1.ReponseMessage = $"cli_Header参数{item.FileName}必填";
                                await ApiSetLog(step1);

                                #endregion 日志记录3 参数转换

                                data.success = false;
                                data.code = "400";
                                data.message = $"连接中心_Header参数{item.FileName}必填";
                                data.exception = null;
                                data.data = null;
                                return ApiResponse(data, agentAPI);
                            }
                            else
                            {
                                //校验Header字段类型 和 值是否相符
                                var itemVal = HttpContext.Request.Headers[item.FileName].ToString();
                                if (!CheckParameterType(item.FileType, itemVal))
                                {
                                    #region 日志记录3 参数转换

                                    step1.EndTime = DateTime.Now;
                                    step1.StepStatus = false;
                                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                    step1.ReponseCode = "400";
                                    step1.ReponseMessage = $"cli_Header参数{item.FileName}字段的值必须是{item.FileType}类型";
                                    await ApiSetLog(step1);

                                    #endregion 日志记录3 参数转换

                                    data.success = false;
                                    data.code = "400";
                                    data.message = $"连接中心_Header参数{item.FileName}字段的值必须是{item.FileType}类型";
                                    data.exception = null;
                                    data.data = null;
                                    return ApiResponse(data, agentAPI);
                                }
                                cli_headerData.Add(item.FileName, itemVal);
                            }
                        }
                        //校验Body中的参数
                        List<JsonPathDto> cli_requestPath = new List<JsonPathDto>();
                        string cli_bodyData = string.Empty;

                        cli_requestPath = GetJsonPath(StrrequestBody);

                        //2 获取配置中的Body入参规则 并且拿到父子级路径
                        var cli_requestbodylist = await _applyApiParameterApp.GetApiRequestBodyData(new EntityDto<Guid> { Id = applyAPI.Id }, false);
                        var cli_ruleBodyPaths = from item in cli_requestbodylist
                                                let path = GetListPath(item, cli_requestbodylist)
                                                select new { item, path };

                        //3 校验前端传值  是否符合 配置规则
                        foreach (var p in cli_ruleBodyPaths)
                        {
                            string thPath = string.Join(".", p.path.Select(i => i.FileName));
                            var requestBodyItem = cli_requestPath.Where(t => t.Name == p.item.FileName && t.Path == thPath);

                            //前置校验是否必填 如果没有传这个必填对象
                            //前置校验 复制结构下 数组套数组 对象套数组等  是否必填的数量 满足 路径
                            if (p.item.IsRequired)
                            {
                                var parentobj = cli_requestbodylist.Where(t => t.Id == p.item.ParentID).FirstOrDefault();
                                var parentItem = cli_requestPath.Where(t => t.Name == parentobj.FileName).FirstOrDefault();
                                //这里计算 这个 必填字段应该有多少个
                                int parentarray = GetParentArrayCount(cli_requestPath, thPath, 1);
                                //如果没有传这个必填对象
                                if (parentarray != requestBodyItem.Count())
                                {
                                    #region 日志记录2 参数校验

                                    step1.EndTime = DateTime.Now;
                                    step1.StepStatus = false;
                                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                    step1.ReponseCode = "400";
                                    step1.ReponseMessage = $"连接中心_Body中参数校验不通过，{thPath}下的{p.item.FileName}字段的值必填，请检查传参格式！";
                                    await ApiSetLog(step1);

                                    #endregion 日志记录2 参数校验

                                    data.success = false;
                                    data.code = "400";
                                    data.message = $"连接中心_Body中参数校验不通过，{thPath}下的{p.item.FileName}字段的值必填，请检查传参格式！";
                                    data.exception = null;
                                    data.data = null;
                                    return ApiResponse(data, agentAPI);
                                }
                            }

                            foreach (var item in requestBodyItem)
                            {
                                if (!CheckParameterType(p.item.FileType, item.Value))
                                {
                                    #region 日志记录3 参数转换

                                    step1.EndTime = DateTime.Now;
                                    step1.StepStatus = false;
                                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                    step1.ReponseCode = "400";
                                    step1.ReponseMessage = $"连接中心_Body中参数校验不通过,{thPath}下的{p.item.FileName}字段的值必须是{p.item.FileType}类型";
                                    await ApiSetLog(step1);

                                    #endregion 日志记录3 参数转换

                                    data.success = false;
                                    data.code = "400";
                                    data.message = $"连接中心_Body中参数校验不通过,{thPath}下的{p.item.FileName}字段的值必须是{p.item.FileType}类型";
                                    data.exception = null;
                                    data.data = null;
                                    return ApiResponse(data, agentAPI);
                                }
                            }
                        }
                        //入参透传 body全传
                        cli_bodyData = StrrequestBody;

                        //http请求参数赋值
                        httpParam.apiUrl = cli_httpUrl.ToString();
                        httpParam.querys = queryParams;
                        httpParam.headers = cli_headerData;
                        httpParam.body = cli_bodyData;
                        httpParam.step1 = step1;

                        #region 日志记录3 参数转换 结束

                        step1.EndTime = DateTime.Now;
                        step1.StepStatus = true;
                        step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                        step1.ReponseCode = "200";
                        step1.ReponseMessage = $"";
                        await ApiSetLog(step1);

                        #endregion 日志记录3 参数转换 结束
                    }

                    if (agentAPI.ParameterModeName == "入参映射")
                    {
                        //查询映射信息(映射信息都会存到这里query header body)
                        var mappingData = await _applyApiParameterApp.GetParameterMappingList(new GetPublicPagesInput
                        {
                            AgentAPIID = agentAPI.Id,
                            Sorting = "id",
                            MaxResultCount = 1000,
                            SkipCount = 0,
                        });
                        //query传参
                        Dictionary<string, string> cli_queryData = new Dictionary<string, string>();
                        var cli_querylist = await _applyApiParameterApp.GetApiQueryData(new EntityDto<Guid> { Id = applyAPI.Id });
                        foreach (var item in (mappingData.Items.Where(t => t.ParameterDataType.ToUpper() == "Query".ToUpper())))
                        {
                            var query_list = cli_querylist.Where(t => t.FileName == item.ParameterName).FirstOrDefault();

                            if (item.MappingMode == "固定值")
                            {
                                query_list.ReceivedValue = item.FixedValue;
                                cli_queryData.Add(item.ParameterName, item.FixedValue);
                                continue;
                            }
                            if (item.MappingMode == "选择")
                            {
                                if (item.MappingDataType.ToUpper() == "Query".ToUpper())
                                {
                                    var thval = querylist.Where(t => t.FileName == item.MappingName).FirstOrDefault();
                                    query_list.ReceivedValue = (thval != null ? thval.ReceivedValue : "");
                                    cli_queryData.Add(item.ParameterName, (thval != null ? thval.ReceivedValue : ""));
                                    continue;
                                }
                                if (item.MappingDataType.ToUpper() == "Header".ToUpper())
                                {
                                    var thval = headerlist.Where(t => t.FileName == item.MappingName).FirstOrDefault();
                                    query_list.ReceivedValue = (thval != null ? thval.ReceivedValue : "");
                                    cli_queryData.Add(item.ParameterName, (thval != null ? thval.ReceivedValue : ""));
                                    continue;
                                }
                                if (item.MappingDataType.ToUpper() == "Body".ToUpper())
                                {
                                    var thval = ruleBodyPaths.Where(t => t.item.FileName == item.MappingName).FirstOrDefault();
                                    query_list.ReceivedValue = (thval != null ? thval.item.ReceivedValue : "");
                                    cli_queryData.Add(item.ParameterName, (thval != null ? thval.item.ReceivedValue : ""));
                                    continue;
                                }
                            }
                        }
                        //query传参 校验
                        foreach (var item in cli_querylist)
                        {
                            //是否必填 校验到底传没有传值
                            if (item.IsRequired && item.ReceivedValue == null)
                            {
                                #region 日志记录3 参数转换

                                step1.EndTime = DateTime.Now;
                                step1.StepStatus = false;
                                step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                step1.ReponseCode = "400";
                                step1.ReponseMessage = $"连接中心_Query参数{item.FileName}必填";
                                await ApiSetLog(step1);

                                #endregion 日志记录3 参数转换

                                data.success = false;
                                data.code = "400";
                                data.message = $"连接中心_Query参数{item.FileName}必填";
                                data.exception = null;
                                data.data = null;
                                return ApiResponse(data, agentAPI);
                            }
                            else
                            {
                                //校验Cli_Query字段类型 和 值是否相符
                                var itemVal = item.ReceivedValue?.ToString();
                                if (!CheckParameterType(item.FileType, itemVal))
                                {
                                    #region 日志记录3 参数转换

                                    step1.EndTime = DateTime.Now;
                                    step1.StepStatus = false;
                                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                    step1.ReponseCode = "400";
                                    step1.ReponseMessage = $"连接中心_Query参数{item.FileName}字段的值必须是{item.FileType}类型";
                                    await ApiSetLog(step1);

                                    #endregion 日志记录3 参数转换

                                    data.success = false;
                                    data.code = "400";
                                    data.message = $"连接中心_Query参数{item.FileName}字段的值必须是{item.FileType}类型";
                                    data.exception = null;
                                    data.data = null;
                                    return ApiResponse(data, agentAPI);
                                }
                            }
                        }

                        //header传参
                        Dictionary<string, string> cli_headerData = new Dictionary<string, string>();
                        var cli_headerlist = await _applyApiParameterApp.GetApiHeaderData(new EntityDto<Guid> { Id = applyAPI.Id });
                        var header_mappingData = mappingData.Items.Where(t => t.ParameterDataType.ToUpper() == "Header".ToUpper());
                        foreach (var item2 in header_mappingData)
                        {
                            var header_list = cli_headerlist.Where(t => t.FileName == item2.ParameterName).FirstOrDefault();
                            if (item2.MappingMode == "固定值")
                            {
                                header_list.ReceivedValue = item2.FixedValue;
                                cli_headerData.Add(item2.ParameterName, item2.FixedValue);
                                continue;
                            }
                            if (item2.MappingMode == "选择")
                            {
                                if (item2.MappingDataType.ToUpper() == "Query".ToUpper())
                                {
                                    var thval = querylist.Where(t => t.FileName == item2.MappingName).FirstOrDefault();
                                    header_list.ReceivedValue = (thval != null ? thval.ReceivedValue : "");
                                    cli_headerData.Add(item2.ParameterName, (thval != null ? thval.ReceivedValue : ""));
                                    continue;
                                }
                                if (item2.MappingDataType.ToUpper() == "Header".ToUpper())
                                {
                                    var thval = headerlist.Where(t => t.FileName == item2.MappingName).FirstOrDefault();
                                    header_list.ReceivedValue = (thval != null ? thval.ReceivedValue : "");
                                    cli_headerData.Add(item2.ParameterName, (thval != null ? thval.ReceivedValue : ""));
                                    continue;
                                }
                                if (item2.MappingDataType.ToUpper() == "Body".ToUpper())
                                {
                                    var thval = ruleBodyPaths.Where(t => t.item.FileName == item2.MappingName).FirstOrDefault();
                                    header_list.ReceivedValue = (thval != null ? thval.item.ReceivedValue : "");
                                    cli_headerData.Add(item2.ParameterName, (thval != null ? thval.item.ReceivedValue : ""));
                                    continue;
                                }
                            }
                        }
                        //header传参 校验
                        foreach (var item in cli_headerlist)
                        {
                            //校验Header字段必填
                            if (item.IsRequired && item.ReceivedValue == null)
                            {
                                #region 日志记录3 参数转换

                                step1.EndTime = DateTime.Now;
                                step1.StepStatus = false;
                                step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                step1.ReponseCode = "400";
                                step1.ReponseMessage = $"连接中心_Header参数{item.FileName}必填";
                                await ApiSetLog(step1);

                                #endregion 日志记录3 参数转换

                                data.success = false;
                                data.code = "400";
                                data.message = $"连接中心_Header参数{item.FileName}必填";
                                data.exception = null;
                                data.data = null;
                                return ApiResponse(data, agentAPI);
                            }
                            else
                            {
                                //校验Header字段类型 和 值是否相符
                                var itemVal = item.ReceivedValue?.ToString();
                                if (!CheckParameterType(item.FileType, itemVal))
                                {
                                    #region 日志记录3 参数转换

                                    step1.EndTime = DateTime.Now;
                                    step1.StepStatus = false;
                                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                    step1.ReponseCode = "400";
                                    step1.ReponseMessage = $"连接中心_Header参数{item.FileName}字段的值必须是{item.FileType}类型";
                                    await ApiSetLog(step1);

                                    #endregion 日志记录3 参数转换

                                    data.success = false;
                                    data.code = "400";
                                    data.message = $"连接中心_Header参数{item.FileName}字段的值必须是{item.FileType}类型";
                                    data.exception = null;
                                    data.data = null;
                                    return ApiResponse(data, agentAPI);
                                }
                            }
                        }

                        //body传参  映射只有的是一个整体
                        string cli_bodyData = string.Empty;
                        var itemboddy = mappingData.Items.Where(t => t.ParameterDataType.ToUpper() == "Body".ToUpper()).FirstOrDefault();
                        if (itemboddy.MappingMode == "固定值")
                        {
                            cli_bodyData = itemboddy.FixedValue;
                        }
                        if (itemboddy.MappingMode == "选择")
                        {
                            if (itemboddy.MappingDataType.ToUpper() == "Query".ToUpper())
                            {
                                var thval = querylist.Where(t => t.FileName == itemboddy.MappingName).FirstOrDefault();
                                cli_bodyData = (thval != null ? thval.ReceivedValue : "");
                            }
                            if (itemboddy.MappingDataType.ToUpper() == "Header".ToUpper())
                            {
                                var thval = headerlist.Where(t => t.FileName == itemboddy.MappingName).FirstOrDefault();
                                cli_bodyData = (thval != null ? thval.ReceivedValue : "");
                            }
                            if (itemboddy.MappingDataType.ToUpper() == "Body".ToUpper())
                            {
                                var thval = ruleBodyPaths.Where(t => t.item.FileName == itemboddy.MappingName).FirstOrDefault();
                                cli_bodyData = (thval != null ? thval.item.ReceivedValue : "");
                            }
                        }

                        //校验Body中的参数
                        List<JsonPathDto> cli_requestPath = new List<JsonPathDto>();
                        cli_requestPath = GetJsonPath(cli_bodyData);

                        // 获取配置中的Body入参规则 并且拿到父子级路径
                        var cli_requestbodylist = await _applyApiParameterApp.GetApiRequestBodyData(new EntityDto<Guid> { Id = applyAPI.Id }, false);
                        var cli_ruleBodyPaths = from item in cli_requestbodylist
                                                let path = GetListPath(item, cli_requestbodylist)
                                                select new { item, path };

                        // 校验前端传值  是否符合 配置规则
                        foreach (var p in cli_ruleBodyPaths)
                        {
                            string thPath = string.Join(".", p.path.Select(i => i.FileName));
                            var requestBodyItem = cli_requestPath.Where(t => t.Name == p.item.FileName && t.Path == thPath);

                            //前置校验是否必填 如果没有传这个必填对象
                            //前置校验 复制结构下 数组套数组 对象套数组等  是否必填的数量 满足 路径
                            if (p.item.IsRequired)
                            {
                                var parentobj = cli_requestbodylist.Where(t => t.Id == p.item.ParentID).FirstOrDefault();
                                var parentItem = cli_requestPath.Where(t => t.Name == parentobj.FileName).FirstOrDefault();
                                //这里计算 这个 必填字段应该有多少个
                                int parentarray = GetParentArrayCount(cli_requestPath, thPath, 1);
                                //如果没有传这个必填对象
                                if (parentarray != requestBodyItem.Count())
                                {
                                    #region 日志记录2 参数校验

                                    step1.EndTime = DateTime.Now;
                                    step1.StepStatus = false;
                                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                    step1.ReponseCode = "400";
                                    step1.ReponseMessage = $"连接中心_Body中参数校验不通过，{thPath}下的{p.item.FileName}字段的值必填，请检查传参格式！";
                                    await ApiSetLog(step1);

                                    #endregion 日志记录2 参数校验

                                    data.success = false;
                                    data.code = "400";
                                    data.message = $"连接中心_Body中参数校验不通过，{thPath}下的{p.item.FileName}字段的值必填，请检查传参格式！";
                                    data.exception = null;
                                    data.data = null;
                                    return ApiResponse(data, agentAPI);
                                }
                            }

                            foreach (var item in requestBodyItem)
                            {
                                //3.1是否必填 校验是否包含这个值  如果包含校验格式是否正确
                                if (!CheckParameterType(p.item.FileType, item.Value))
                                {
                                    #region 日志记录3 参数转换

                                    step1.EndTime = DateTime.Now;
                                    step1.StepStatus = false;
                                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                    step1.ReponseCode = "400";
                                    step1.ReponseMessage = $"连接中心_Body中参数校验不通过,{thPath}下的{p.item.FileName}字段的值必须是{p.item.FileType}类型";
                                    await ApiSetLog(step1);

                                    #endregion 日志记录3 参数转换

                                    data.success = false;
                                    data.code = "400";
                                    data.message = $"连接中心_Body中参数校验不通过,{thPath}下的{p.item.FileName}字段的值必须是{p.item.FileType}类型";
                                    data.exception = null;
                                    data.data = null;
                                    return ApiResponse(data, agentAPI);
                                }
                            }
                        }

                        //http请求参数赋值
                        httpParam.apiUrl = cli_httpUrl.ToString();
                        httpParam.querys = cli_queryData;
                        httpParam.headers = cli_headerData;
                        httpParam.body = cli_bodyData;
                        httpParam.step1 = step1;

                        #region 日志记录3 参数转换 结束

                        step1.EndTime = DateTime.Now;
                        step1.StepStatus = true;
                        step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                        step1.ReponseCode = "200";
                        step1.ReponseMessage = $"";
                        await ApiSetLog(step1);

                        #endregion 日志记录3 参数转换 结束
                    }

                    #region 日志记录4 http返回 开始

                    step1.Id = Guid.NewGuid();
                    step1.StepName = "请求后台服务";
                    step1.StepSort = 40;
                    step1.StepType = "请求后台服务";
                    step1.StartTime = DateTime.Now;
                    step1.RequestSourceIP = null;
                    step1.RequestURL = cli_httpUrl.ToString();
                    step1.RequestMethod = agentAPI.ApplyAPIMethodCode.ToUpper();
                    var strQuery = httpParam.querys
                                 .Select(q => $"{q.Key}={q.Value}")
                                 .ToJsonString();
                    step1.RequestQuerys = string.Join("\n", strQuery);

                    var strHeader = httpParam.headers
                            .Select(q => $"{q.Key}={q.Value}")
                            .ToJsonString();
                    step1.RequestHeaders = string.Join("\n", strHeader);
                    step1.RequestBody = httpParam.body;

                    #endregion 日志记录4 http返回 开始

                    //处理请求
                    string http_responseData = string.Empty;
                    try
                    {
                        if (agentAPI.ApplyAPIMethodCode.ToUpper() == "POST".ToUpper())
                        {
                            return await HttpPost(httpParam);
                        }
                        else if (agentAPI.ApplyAPIMethodCode.ToUpper() == "GET".ToUpper())
                        {
                            return await HttpGet(httpParam);
                        }
                    }
                    catch (Exception ex)
                    {
                        data.success = false;
                        data.code = "500";
                        data.message = $"请求失败:{ex.Message}";
                        data.exception = ex;
                        data.data = http_responseData;

                        #region 日志记录4 http返回 异常结束

                        step1 = httpParam.step1;
                        step1.EndTime = DateTime.Now;
                        step1.StepStatus = false;
                        step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();
                        step1.ReponseCode = "500";
                        step1.ReponseMessage = $"{ex.Message}";
                        step1.ReponseHeaders = null;
                        step1.ResponseBody = JsonConvert.SerializeObject(data);
                        await ApiSetLog(step1, applyHealthTesting);

                        #endregion 日志记录4 http返回 异常结束

                        return ApiResponse(data, agentAPI);
                    }
                }
            }
            //校验通过之后判断  连接器是WebService
            if (agentAPI.ServiceName.ToLower() == "WebService".ToLower())
            {
                #region 日志记录3 参数转换 开始

                step1.Id = Guid.NewGuid();
                step1.StepName = "参数转换";
                step1.StepSort = 30;
                step1.StepType = "参数转换";
                step1.StartTime = DateTime.Now;

                #endregion 日志记录3 参数转换 开始

                //查询映射信息(映射信息都会存到这里query header body)
                var mappingData = await _applyApiParameterApp.GetParameterMappingList(new GetPublicPagesInput
                {
                    AgentAPIID = agentAPI.Id,
                    Sorting = "id",
                    MaxResultCount = 1000,
                    SkipCount = 0,
                });
                //http请求入参对象
                HttpParamDto httpParam = new HttpParamDto();
                httpParam.applyHealth = applyHealthTesting;
                httpParam.Reponsedata = data;
                httpParam.ApplyID = apply.Id;
                httpParam.agentAPI = agentAPI;
                //实时获取webservice的服务数据
                var webservice = await _webServiceAppService.GetWbeServiceWsdl(apply.Id);
                if (webservice?.Count > 0)
                {
                    var serviceMethod = webservice.Where(t => t.InterfaceName == agentAPI.ApplyAPIPath).ToList().FirstOrDefault();
                    if (serviceMethod == null)
                    {
                        #region 日志记录3 参数转换 结束

                        step1.EndTime = DateTime.Now;
                        step1.StepStatus = false;
                        step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();
                        step1.ReponseCode = "400";
                        step1.ReponseMessage = $"解析wsdl失败:没有包含该方法{agentAPI.ApplyAPIPath}，连接应用Code={apply.ApplyCode},返回数据wsdl={JsonConvert.SerializeObject(webservice)}";
                        await ApiSetLog(step1);

                        #endregion 日志记录3 参数转换 结束

                        return ApiResponse(data, agentAPI);
                    }
                    //请求头
                    Dictionary<string, string> cli_headerData = new Dictionary<string, string>();
                    cli_headerData.Add("SOAPAction", serviceMethod?.SOAPAction);
                    //请求Body
                    string web_RequestBody = serviceMethod?.RequestBody;
                    //请求地址
                    string cli_Url = serviceMethod?.Path;

                    //替换 请求参数
                    foreach (var item in mappingData.Items)
                    {
                        if (item.MappingMode == "固定值")
                        {
                            web_RequestBody = web_RequestBody.Replace("@" + item.ParameterName, item.FixedValue);
                            continue;
                        }
                        if (item.MappingMode == "选择")
                        {
                            if (item.MappingDataType.ToUpper() == "Query".ToUpper())
                            {
                                var thval = querylist.Where(t => t.FileName == item.MappingName).FirstOrDefault();
                                web_RequestBody = web_RequestBody.Replace("@" + item.ParameterName, (thval != null ? thval.ReceivedValue : ""));
                                continue;
                            }
                            if (item.MappingDataType.ToUpper() == "Header".ToUpper())
                            {
                                var thval = headerlist.Where(t => t.FileName == item.MappingName).FirstOrDefault();
                                web_RequestBody = web_RequestBody.Replace("@" + item.ParameterName, (thval != null ? thval.ReceivedValue : ""));
                                continue;
                            }
                            if (item.MappingDataType.ToUpper() == "Body".ToUpper())
                            {
                                var thval = ruleBodyPaths.Where(t => t.item.FileName == item.MappingName).FirstOrDefault();
                                web_RequestBody = web_RequestBody.Replace("@" + item.ParameterName, (thval != null ? thval.item.ReceivedValue : ""));
                                continue;
                            }
                        }
                    }

                    //http请求参数赋值
                    httpParam.apiUrl = cli_Url;
                    httpParam.querys = null;
                    httpParam.headers = cli_headerData;
                    httpParam.body = null;
                    httpParam.xmlRequest = web_RequestBody;
                    httpParam.SOAPAction = serviceMethod?.SOAPAction;
                    httpParam.step1 = step1;

                    #region 日志记录3 参数转换 结束

                    step1.EndTime = DateTime.Now;
                    step1.StepStatus = true;
                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                    step1.ReponseCode = "200";
                    step1.ReponseMessage = $"";
                    await ApiSetLog(step1);

                    #endregion 日志记录3 参数转换 结束

                    #region 日志记录4 http返回 开始

                    step1.Id = Guid.NewGuid();
                    step1.StepName = "请求后台服务";
                    step1.StepSort = 40;
                    step1.StepType = "请求后台服务";
                    step1.StartTime = DateTime.Now;
                    step1.RequestSourceIP = null;
                    step1.RequestURL = cli_Url;
                    step1.RequestMethod = agentAPI.ApplyAPIMethodCode.ToUpper();
                    step1.RequestQuerys = null;
                    var strHeader = cli_headerData
                            .Select(q => $"{q.Key}={q.Value}")
                            .ToJsonString();
                    step1.RequestHeaders = string.Join("\n", strHeader);
                    step1.RequestBody = web_RequestBody;

                    #endregion 日志记录4 http返回 开始

                    //处理请求
                    string http_responseData = string.Empty;
                    try
                    {
                        return await CallWebService(httpParam);
                    }
                    catch (Exception ex)
                    {
                        data.success = false;
                        data.code = "500";
                        data.message = $"请求失败:{ex.Message}";
                        data.exception = ex;
                        data.data = http_responseData;

                        #region 日志记录4 http返回 异常结束

                        step1 = httpParam.step1;
                        step1.EndTime = DateTime.Now;
                        step1.StepStatus = false;
                        step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();
                        step1.ReponseCode = "500";
                        step1.ReponseMessage = $"{ex.Message}";
                        step1.ReponseHeaders = null;
                        step1.ResponseBody = JsonConvert.SerializeObject(data);
                        await ApiSetLog(step1, applyHealthTesting);

                        #endregion 日志记录4 http返回 异常结束

                        return ApiResponse(data, agentAPI);
                    }
                }
                else
                {
                    #region 日志记录3 参数转换 结束

                    step1.EndTime = DateTime.Now;
                    step1.StepStatus = false;
                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();
                    step1.ReponseCode = "400";
                    step1.ReponseMessage = $"解析wsdl失败:连接应用Code={apply.ApplyCode},返回数据wsdl={JsonConvert.SerializeObject(webservice)}";
                    await ApiSetLog(step1);

                    #endregion 日志记录3 参数转换 结束

                    return ApiResponse(data, agentAPI);
                }
            }
            //校验通过之后判断 连接器是SqlServer
            if (agentAPI.ServiceName.ToLower() == "sqlserver".ToLower()
                || agentAPI.ServiceName.ToLower() == "mysql".ToLower()
                || agentAPI.ServiceName.ToLower() == "oracle".ToLower())
            {
                #region 日志记录3 参数转换 开始

                step1.Id = Guid.NewGuid();
                step1.StepName = "参数转换";
                step1.StepSort = 30;
                step1.StepType = "参数转换";
                step1.StartTime = DateTime.Now;

                #endregion 日志记录3 参数转换 开始

                //分页页码
                int pageSize = 0;
                int pageIndex = 0;
                if (apply != null)
                {
                    List<SugarParameter> sugarParameter = new List<SugarParameter>();
                    //查询映射信息
                    var mappingData = await _applyApiParameterApp.GetParameterMappingList(new GetPublicPagesInput
                    {
                        AgentAPIID = agentAPI.Id,
                        Sorting = "id",
                        MaxResultCount = 1000,
                        SkipCount = 0,
                    });
                    foreach (var item in mappingData.Items)
                    {
                        if (item.ParameterDataType.ToLower() != "sql_query".ToLower() && item.ParameterDataType.ToLower() != "sql_page".ToLower())
                        {
                            continue;
                        }
                        string thval = string.Empty;
                        if (item.MappingMode == "固定值")
                        {
                            thval = item.FixedValue;
                        }
                        else if (item.MappingMode == "选择")
                        {
                            if (item.MappingDataType.ToUpper() == "Query".ToUpper())
                            {
                                var thitem = querylist.Where(t => t.FileName == item.MappingName).FirstOrDefault();
                                thval = (thitem != null ? thitem.ReceivedValue : "");
                            }
                            else if (item.MappingDataType.ToUpper() == "Header".ToUpper())
                            {
                                var thitem = headerlist.Where(t => t.FileName == item.MappingName).FirstOrDefault();
                                thval = (thitem != null ? thitem.ReceivedValue : "");
                            }
                            else if (item.MappingDataType.ToUpper() == "Body".ToUpper())
                            {
                                var thitem = ruleBodyPaths.Where(t => t.item.FileName == item.MappingName).FirstOrDefault();
                                thval = (thitem != null ? thitem.item.ReceivedValue : "");
                            }
                        }

                        //参数条件 替换
                        if (item.ParameterDataType.ToLower() == "sql_query".ToLower())
                        {
                            sugarParameter.Add(new SugarParameter(item.ParameterName.Replace("@", ""), thval));
                        }//分页条件 赋值
                        else if (item.ParameterDataType.ToLower() == "sql_page".ToLower())
                        {
                            if (item.ParameterName == "pageSize")
                            {
                                int.TryParse(thval, out pageSize);
                            }
                            if (item.ParameterName == "pageIndex")
                            {
                                int.TryParse(thval, out pageIndex);

                                if (pageIndex < 0)
                                {
                                    #region 日志记录3 参数转换

                                    step1.EndTime = DateTime.Now;
                                    step1.StepStatus = false;
                                    step1.StepTime = (step1.EndTime - step1.StartTime).TotalMilliseconds.ToString();

                                    step1.ReponseCode = "400";
                                    step1.ReponseMessage = $"pageIndex起始页码应该从0或者1开始";
                                    await ApiSetLog(step1);

                                    #endregion 日志记录3 参数转换

                                    data.success = false;
                                    data.code = "400";
                                    data.message = $"pageIndex起始页码应该从0或者1开始";
                                    data.exception = null;
                                    data.data = null;
                                    return ApiResponse(data, agentAPI);
                                }
                            }
                        }
                    }
                    SqlserverParamDto sqlserverParam = new SqlserverParamDto();
                    sqlserverParam.apply = apply;
                    sqlserverParam.agentAPI = agentAPI;
                    sqlserverParam.pageSize = pageSize;
                    sqlserverParam.pageIndex = pageIndex;
                    sqlserverParam.steplog = step1;
                    sqlserverParam.reponseData = data;
                    sqlserverParam.sugarParameter = sugarParameter;
                    sqlserverParam.applyHealthTesting = applyHealthTesting;

                    object pageQuery = await SqlDataMontage(sqlserverParam);
                    data.success = true;
                    data.code = "200";
                    data.message = "";
                    data.exception = null;
                    data.data = pageQuery;
                    return ApiResponse(data, agentAPI);
                }
            }

            #region 日志记录 返回 结束

            await ApiSetLog(step1);

            #endregion 日志记录 返回 结束

            return ApiResponse(data, agentAPI);
        }

        /// <summary>
        /// 校验参数类型
        /// </summary>
        /// <returns></returns>
        private bool CheckParameterType(string type, string value)
        {
            switch (type)
            {
                case "object":
                    return true;

                case "string":
                    return true;

                case "number":
                    return decimal.TryParse(value, out decimal result1);

                case "integer":
                    return int.TryParse(value, out int result2);

                case "boolean":
                    return bool.TryParse(value, out bool result3);

                case "arrayOfObject":
                    return JsonParse(type, value);

                case "arrayOfString":
                    return JsonParse(type, value);

                case "arrayOfNumber":
                    return JsonParse(type, value);

                case "arrayOfInteger":
                    return JsonParse(type, value);

                case "arrayOfBoolean":
                    return JsonParse(type, value);

                default:
                    return true;
            }
        }

        /// <summary>
        /// 校验数组array类型判断
        /// </summary>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        private bool JsonParse(string type, string value)
        {
            try
            {
                value = value.Replace("\r", "").Replace("\n", "").Replace(" ", "").Replace("\\\"", "\"");
                JsonDocument doc = JsonDocument.Parse(value);
                if (doc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    switch (type)
                    {
                        case "arrayOfObject":
                            return true;

                        case "arrayOfString":
                            return true;

                        case "arrayOfNumber":
                            decimal[] array1 = System.Text.Json.JsonSerializer.Deserialize<string[]>(value)
                                          .Select(s => decimal.Parse(s))
                                          .ToArray();
                            return true;

                        case "arrayOfInteger":
                            int[] array2 = System.Text.Json.JsonSerializer.Deserialize<string[]>(value)
                                       .Select(s => int.Parse(s))
                                       .ToArray();
                            return true;

                        case "arrayOfBoolean":
                            bool[] array = System.Text.Json.JsonSerializer.Deserialize<string[]>(value)
                                 .Select(s => bool.Parse(s))
                                 .ToArray();
                            return true;

                        default:
                            return true;
                    }
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                return false;
            };
        }

        /// <summary>
        /// 获取JSON字符串中所有属性的父子级关系路径
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private List<JsonPathDto> GetJsonPath(string path)
        {
            List<JsonPathDto> jsonPaths = new List<JsonPathDto>();
            JObject obj = null;
            if (string.IsNullOrWhiteSpace(path))
            {
                return jsonPaths;
            }
            jsonPaths.Add(new JsonPathDto
            {
                Name = "RequestBody",
                Value = path,
                Path = "RequestBody"
            }); ;
            Action<JToken, string> printPaths = null;
            printPaths = (jToken, path) =>
            {
                if (jToken is JProperty prop)
                {
                    //Console.WriteLine($"{path}.{prop.Name}");
                    printPaths(prop.Value, $"{path}.{prop.Name}");
                    JsonPathDto jsonPath = new JsonPathDto();
                    jsonPath.Name = prop.Name;
                    jsonPath.Value = prop.Value.ToString();
                    jsonPath.Path = $"RequestBody{path}.{prop.Name}";
                    jsonPaths.Add(jsonPath);
                }
                else if (jToken is JContainer container)
                {
                    foreach (JToken child in container.Children())
                    {
                        printPaths(child, path);
                    }
                }
            };
            try
            {
                JsonDocument doc = JsonDocument.Parse(path);
                if (doc.RootElement.ValueKind == JsonValueKind.Object)
                {
                    obj = JObject.Parse(path);
                    printPaths(obj, "");
                }
                else if (doc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    foreach (JsonElement element in doc.RootElement.EnumerateArray())
                    {
                        obj = JObject.Parse(element.GetRawText());
                        printPaths(obj, "");
                    }
                }
            }
            catch (Exception)
            {
                return jsonPaths;
            }
            return jsonPaths;
        }

        /// <summary>
        /// 获取List对象中所有属性的父子级关系路径
        /// </summary>
        /// <param name="item"></param>
        /// <param name="listDtos"></param>
        /// <returns></returns>
        private IEnumerable<ApplyBodyDataListDto> GetListPath(ApplyBodyDataListDto item, List<ApplyBodyDataListDto> listDtos)
        {
            if (item.ParentID == null || item.ParentID == Guid.Empty)
            {
                return new[] { item };
            }
            else
            {
                var parent = listDtos.Single(i => i.Id == item.ParentID);
                return GetListPath(parent, listDtos).Concat(new[] { item });
            }
        }

        /// <summary>
        /// 获取当前array对象的 length
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private int GetArrayCount(string path)
        {
            int arraycount = 1;
            try
            {
                JsonDocument doc = JsonDocument.Parse(path);

                if (doc.RootElement.ValueKind == JsonValueKind.Array)
                {
                    arraycount = doc.RootElement.EnumerateArray().Count();
                }
            }
            catch (Exception)
            {
                return arraycount;
            }
            return arraycount;
        }

        private int GetParentArrayCount(List<JsonPathDto> json, string path, int array)
        {
            try
            {
                var data = json.Where(t => t.Path == path).FirstOrDefault();
                if (data != null)
                {
                    JsonDocument doc = JsonDocument.Parse(data.Value);
                    if (doc.RootElement.ValueKind == JsonValueKind.Array)
                    {
                        array *= doc.RootElement.EnumerateArray().Count();
                    }

                    int lastIndex = path.LastIndexOf('.');
                    if (lastIndex >= 0)
                    {
                        path = path.Substring(0, lastIndex);
                        array = GetParentArrayCount(json, path, array);
                    }
                }
                return array;
            }
            catch (Exception)
            {
                int lastIndex = path.LastIndexOf('.');
                if (lastIndex >= 0)
                {
                    path = path.Substring(0, lastIndex);
                    return array = GetParentArrayCount(json, path, array);
                }
                else
                {
                    return array;
                }
            }
        }

        /// <summary>
        /// SQL数据拼接 & 执行
        /// </summary>
        /// <returns></returns>
        private async Task<object> SqlDataMontage(SqlserverParamDto paramDto)
        {
            ResponseDataDto dataDto = new ResponseDataDto();
            //参数
            string sqlType = paramDto.apply.SqlType;
            int SqlPort = paramDto.apply.SqlPortTest;
            string SqlHost = paramDto.apply.SqlHostTest;
            string sqlDatabase = paramDto.apply.SqlDBTest;
            string sqlUserName = paramDto.apply.SqlUserNameTest;
            string sqlPassWord = paramDto.apply.SqlPassWordTest;
            if (_environment.IsProduction())
            {
                SqlHost = paramDto.apply.SqlHostProduce;
                SqlPort = paramDto.apply.SqlPortProduce;
                sqlDatabase = paramDto.apply.SqlDBProduce;
                sqlUserName = paramDto.apply.SqlUserNameProduce;
                sqlPassWord = paramDto.apply.SqlPassWordProduce;
            }

            int totalpage = 0;
            int skipCount = paramDto.agentAPI.StartPage;
            bool isPaging = paramDto.agentAPI.IsPaging;
            //查询语句
            string sqlQuery = paramDto.agentAPI.SqlQuery;
            //排序
            var sortlist = JsonConvert.DeserializeObject<List<SotrDto>>(paramDto.agentAPI.SortJson);
            string sortSql = string.Empty;
            foreach (var item in sortlist)
            {
                if (sortSql != string.Empty)
                {
                    sortSql += $",{item.Name} {item.Sort}";
                }
                else
                {
                    sortSql += $" {item.Name} {item.Sort}";
                }
            }
            //如果没指定排序 给ID默认排序
            sortSql = sortSql == string.Empty ? " id ASC " : sortSql;

            ///是否分页查询 不启用 默认查前200条数据
            if (!isPaging)
            {
                paramDto.pageIndex = 1;
                paramDto.pageSize = 200;
            }

            StringBuilder sqlBui_Count = new StringBuilder();
            StringBuilder sqlBui = new StringBuilder();

            if (sqlType.ToLower() == "sqlserver".ToLower())
            {
                //一共多少页
                sqlBui_Count.Append($"SELECT COUNT(*) maxCount FROM ({sqlQuery}) AS maxCount");

                //分页默认是从0开始
                sqlBui.Append($" SELECT ");
                sqlBui.Append($" * FROM (SELECT ROW_NUMBER() OVER (ORDER BY {sortSql}) AS row_number,* FROM ");
                sqlBui.Append($" ({sqlQuery}) AS items ) AS subquery WHERE row_number ");
                sqlBui.Append($"  BETWEEN ((@pageIndex) * @pageSize + 1) AND ((@pageIndex+1) * @pageSize)");
                sqlBui.Append($" ORDER BY {sortSql}; ");

                //从0开始：PageIndex=0时，是第一页；PageIndex=1时，是第二页；以此类推...
                //从1开始：PageIndex = 1时，是第一页；PageIndex = 2时，是第二页；以此类推...

                int pageindex = paramDto.pageIndex;
                int pageSize = paramDto.pageSize;
                if (skipCount == 1)
                {
                    pageindex = pageindex - 1;
                    pageindex = pageindex < 0 ? 0 : pageindex;
                }
                paramDto.sugarParameter.Add(new SugarParameter("pageIndex", pageindex));
                paramDto.sugarParameter.Add(new SugarParameter("pageSize", pageSize));
            }

            if (sqlType.ToLower() == "mysql".ToLower())
            {
                //一共多少页
                sqlBui_Count.Append($"SELECT COUNT(*) maxCount FROM ({sqlQuery}) AS maxCount");

                //分页默认是从0开始
                sqlBui.Append($" SELECT  ");
                sqlBui.Append($" sys_tag_t.*  ");
                sqlBui.Append($" FROM ({sqlQuery}) AS sys_tag_t  ");
                sqlBui.Append($" ORDER BY {sortSql}  ");
                sqlBui.Append($" LIMIT @pageIndex,@pageSize;  ");

                //从0开始：PageIndex=0时，是第一页；PageIndex=1时，是第二页；以此类推...
                //从1开始：PageIndex = 1时，是第一页；PageIndex = 2时，是第二页；以此类推...
                int pageindex = paramDto.pageIndex;
                int pageSize = paramDto.pageSize;
                if (skipCount == 1)
                {
                    pageindex = pageindex - 1;
                    pageindex = pageindex < 0 ? 0 : pageindex;
                }
                pageindex = (pageindex * pageSize);

                paramDto.sugarParameter.Add(new SugarParameter("pageIndex", pageindex));
                paramDto.sugarParameter.Add(new SugarParameter("pageSize", pageSize));
            }

            if (sqlType.ToLower() == "oracle".ToLower())
            {
                int pageindex = paramDto.pageIndex;
                int pageSize = paramDto.pageSize;
                if (skipCount == 1)
                {
                    pageindex = pageindex - 1;
                    pageindex = pageindex < 0 ? 0 : pageindex;
                }
                pageSize = (pageindex + 1) * paramDto.pageSize;
                pageindex = pageindex * paramDto.pageSize;

                //一共多少页
                sqlBui_Count.Append($"SELECT COUNT(*) maxCount FROM ({sqlQuery})");

                //分页默认是从0开始
                sqlBui.Append($" SELECT ");
                sqlBui.Append($" sys_tag_t.* ");
                sqlBui.Append($" FROM ( ");
                sqlBui.Append($"  SELECT sys_tag_t.*, ROWNUM AS row_number ");
                sqlBui.Append($" FROM ({sqlQuery} ORDER BY {sortSql}) sys_tag_t   ");
                sqlBui.Append($" WHERE ROWNUM <= ({pageSize}) ");
                sqlBui.Append($" ) sys_tag_t ");
                sqlBui.Append($" WHERE row_number > ({pageindex})");
            }

            #region 日志记录3 参数转换 结束

            paramDto.steplog.EndTime = DateTime.Now;
            paramDto.steplog.StepStatus = true;
            paramDto.steplog.StepTime = (paramDto.steplog.EndTime - paramDto.steplog.StartTime).TotalMilliseconds.ToString();

            paramDto.steplog.ReponseCode = "200";
            paramDto.steplog.ReponseMessage = $"";
            await ApiSetLog(paramDto.steplog);

            #endregion 日志记录3 参数转换 结束

            #region 日志记录4 sqlserver返回 开始

            paramDto.steplog.Id = Guid.NewGuid();
            paramDto.steplog.StepName = "请求后台服务";
            paramDto.steplog.StepSort = 40;
            paramDto.steplog.StepType = "请求后台服务";
            paramDto.steplog.StartTime = DateTime.Now;
            paramDto.steplog.RequestSourceIP = null;
            paramDto.steplog.RequestURL = SqlHost;
            paramDto.steplog.RequestMethod = "Sqlserver";
            paramDto.steplog.RequestQuerys = null;
            paramDto.steplog.RequestHeaders = JsonConvert.SerializeObject(paramDto.sugarParameter);
            paramDto.steplog.RequestBody = sqlBui.ToString();

            #endregion 日志记录4 sqlserver返回 开始

            DataTable countData = new DataTable();
            DataTable pageData = new DataTable();
            try
            {
                var connectionString = _sqlHelper.ToConnectionString(sqlType, SqlHost, SqlPort, sqlUserName, sqlPassWord, sqlDatabase);
                countData = await _sqlHelper.GetPaged(sqlType, connectionString, sqlBui_Count.ToString(), paramDto.sugarParameter.ToArray());
                pageData = await _sqlHelper.GetPaged(sqlType, connectionString, sqlBui.ToString(), paramDto.sugarParameter.ToArray());
            }
            catch (Exception ex)
            {
                #region 日志记录4 sqlserver返回 异常结束

                paramDto.steplog.EndTime = DateTime.Now;
                paramDto.steplog.StepStatus = false;
                paramDto.steplog.StepTime = (paramDto.steplog.EndTime - paramDto.steplog.StartTime).TotalMilliseconds.ToString();
                paramDto.steplog.ReponseCode = "500";
                paramDto.steplog.ReponseMessage = $"{ex.Message}";
                paramDto.steplog.ReponseHeaders = null;
                paramDto.steplog.ResponseBody = JsonConvert.SerializeObject(dataDto);
                await ApiSetLog(paramDto.steplog, paramDto.applyHealthTesting);

                #endregion 日志记录4 sqlserver返回 异常结束

                return "";
            }

            //处理返回数据 总行数
            if (countData.Rows.Count > 0)
            {
                int.TryParse(countData.Rows[0]["maxCount"].ToString(), out totalpage);
                dataDto.totalpage = totalpage;
            }

            //处理返回数据
            dataDto.pageSize = paramDto.pageSize;
            dataDto.pageIndex = paramDto.pageIndex;
            dataDto.skipCount = ($"起始页码,从({skipCount})开始");
            if (pageData.Rows.Count > 0)
            {
                dataDto.ReponseData = pageData;
            }

            #region 日志记录4 sqlserver返回 结束

            paramDto.reponseData.success = true;
            paramDto.reponseData.code = "200";
            paramDto.reponseData.message = "";
            paramDto.reponseData.exception = null;
            paramDto.reponseData.data = dataDto;

            paramDto.steplog.EndTime = DateTime.Now;
            paramDto.steplog.StepStatus = true;
            paramDto.steplog.StepTime = (paramDto.steplog.EndTime - paramDto.steplog.StartTime).TotalMilliseconds.ToString();
            paramDto.steplog.ReponseCode = "200";
            paramDto.steplog.ReponseMessage = $"";
            paramDto.steplog.ReponseHeaders = null;
            paramDto.steplog.ResponseBody = JsonConvert.SerializeObject(paramDto.reponseData);
            await ApiSetLog(paramDto.steplog, paramDto.applyHealthTesting);

            #endregion 日志记录4 sqlserver返回 结束

            return dataDto;
        }

        /// <summary>
        /// http post请求
        /// </summary>
        /// <returns></returns>
        private async Task<IActionResult> HttpPost(HttpParamDto httpParam)
        {
            // 添加Cookie到HttpClient实例中
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.CookieContainer = _cookieContainer;
            using (var httpClient = new HttpClient(httpClientHandler))
            {
                //链接超时策略
                httpClient.Timeout = HttpConnectionTimedOut(httpParam.applyHealth);
                var baseAddress = new Uri(httpParam.apiUrl);
                httpClient.BaseAddress = new Uri(baseAddress.Scheme + "://" + baseAddress.Host);
                //URL，包含 query 参数
                StringBuilder cli_querData = new StringBuilder();
                IDictionary<string, string> cli_queryData = new Dictionary<string, string>();
                foreach (var item in httpParam.querys)
                {
                    if (cli_querData.Length <= 0)
                    {
                        cli_querData.Append($"?");
                    }
                    else
                    {
                        cli_querData.Append($"&");
                    }
                    cli_querData.Append($"{item.Key}={item.Value}");
                };

                // 构造请求头 header 参数
                foreach (var item in httpParam.headers)
                {
                    httpClient.DefaultRequestHeaders.Add(item.Key, item.Value);
                };

                // 构造请求体 body 参数
                var requestBodyJson = httpParam.body;
                var requestBodyContent = new StringContent(requestBodyJson, Encoding.UTF8, "application/json");

                //httpParam.applyHealth
                //从健康策略中读取重试次数 & 重试时间间隔
                int retryNumber = 0;
                int retryTimeSpan = 0;
                if (httpParam.applyHealth.RetryPolicyName == "限制重试次数")
                {
                    retryNumber = httpParam.applyHealth.RetryNumber;
                    retryTimeSpan = TimeConversion(httpParam.applyHealth.RetryInterval, httpParam.applyHealth.RITypeName);
                }
                else if (httpParam.applyHealth.RetryPolicyName == "无限重试")
                {
                    //无线重试 上线100
                    retryNumber = 100;
                    retryTimeSpan = 5;
                }

                // 设置重试策略
                var retryPolicy = Policy.HandleResult<HttpResponseMessage>(reponse => !reponse.IsSuccessStatusCode)
                                .Or<TimeoutException>()
                                .WaitAndRetryAsync(retryNumber, retryAttempt => TimeSpan.FromSeconds(retryTimeSpan));

                //重试次数
                int retry = -1;
                var baseresponse = await retryPolicy.ExecuteAsync(async () =>
                {
                    retry++;
                    // 发送 POST 请求
                    return await httpClient.PostAsync(httpParam.apiUrl, requestBodyContent);
                });

                var headerString = baseresponse.Headers
                    .Select(h => $"{h.Key}: {h.Value}")
                    .ToJsonString();
                httpParam.step1.ReponseHeaders = string.Join("\n", headerString);

                // 从登录响应中提取Cookie，并将其保存在本地
                if (baseresponse.Headers.TryGetValues("Set-Cookie", out var cookieValues))
                {
                    foreach (var cookieValue in cookieValues)
                    {
                        var issetCookie = SetCookieHeaderValue.TryParse(cookieValue, out SetCookieHeaderValue mycokie);
                        //格式要是不正确直接跳过
                        if (issetCookie)
                        {
                            var cookie = ToCookie(mycokie, baseAddress.Host.ToString());

                            Response.Cookies.Append(cookie.Name, cookie.ToString());
                            _cookieContainer.Add(httpClient.BaseAddress, cookie);
                        }
                    }
                }

                // 处理响应
                if (baseresponse.IsSuccessStatusCode)
                {
                    var responseJson = await baseresponse.Content.ReadAsStringAsync();
                    httpParam.Reponsedata.success = true;
                    httpParam.Reponsedata.code = "200";
                    httpParam.Reponsedata.message = $"请求成功";
                    httpParam.Reponsedata.exception = null;
                    httpParam.Reponsedata.data = JsonConvert.DeserializeObject<object>(responseJson);

                    #region 日志记录4 http返回 结束

                    httpParam.step1 = httpParam.step1;
                    httpParam.step1.EndTime = DateTime.Now;
                    httpParam.step1.StepStatus = true;
                    httpParam.step1.StepTime = (httpParam.step1.EndTime - httpParam.step1.StartTime).TotalMilliseconds.ToString();
                    httpParam.step1.ReponseCode = "200";
                    httpParam.step1.ReponseMessage = $"重试次数({retry})，重试间隔时间（{retryTimeSpan}秒）";
                    httpParam.step1.ReponseHeaders = null;
                    httpParam.step1.ResponseBody = JsonConvert.SerializeObject(httpParam.Reponsedata);
                    await ApiSetLog(httpParam.step1, httpParam.applyHealth);

                    #endregion 日志记录4 http返回 结束

                    return ApiResponse(httpParam.Reponsedata, httpParam.agentAPI);
                }
                else
                {
                    var responseJson = await baseresponse.Content.ReadAsStringAsync();

                    httpParam.Reponsedata.success = false;
                    httpParam.Reponsedata.code = "500";
                    httpParam.Reponsedata.message = $"请求失败";
                    httpParam.Reponsedata.exception = null;
                    httpParam.Reponsedata.data = JsonConvert.DeserializeObject<object>(responseJson);

                    #region 日志记录4 http返回 结束

                    httpParam.step1 = httpParam.step1;
                    httpParam.step1.EndTime = DateTime.Now;
                    httpParam.step1.StepStatus = false;
                    httpParam.step1.StepTime = (httpParam.step1.EndTime - httpParam.step1.StartTime).TotalMilliseconds.ToString();
                    httpParam.step1.ReponseCode = "500";
                    httpParam.step1.ReponseMessage = $"重试次数({retry})，重试间隔时间（{retryTimeSpan}秒）";
                    httpParam.step1.ReponseHeaders = null;
                    httpParam.step1.ResponseBody = JsonConvert.SerializeObject(httpParam.Reponsedata);
                    await ApiSetLog(httpParam.step1, httpParam.applyHealth);

                    #endregion 日志记录4 http返回 结束

                    return ApiResponse(httpParam.Reponsedata, httpParam.agentAPI);
                }
            }
        }

        /// <summary>
        /// http get请求
        /// </summary>
        /// <param name="url"></param>
        /// <param name="headers"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        private async Task<IActionResult> HttpGet(HttpParamDto httpParam)
        {
            // 添加Cookie到HttpClient实例中
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.CookieContainer = _cookieContainer;
            using (var httpClient = new HttpClient(httpClientHandler))
            {
                //链接超时策略
                httpClient.Timeout = HttpConnectionTimedOut(httpParam.applyHealth);
                var baseAddress = new Uri(httpParam.apiUrl);
                httpClient.BaseAddress = new Uri(baseAddress.Scheme + "://" + baseAddress.Host);

                var uriBuilder = new UriBuilder(httpParam.apiUrl);

                //query
                var query = HttpUtility.ParseQueryString(uriBuilder.Query);
                foreach (var item in httpParam.querys)
                {
                    query[item.Key] = item.Value;
                };
                uriBuilder.Query = query.ToString();

                //httpParam.applyHealth
                //从健康策略中读取重试次数 & 重试时间间隔
                int retryNumber = 0;
                int retryTimeSpan = 0;
                if (httpParam.applyHealth.RetryPolicyName == "限制重试次数")
                {
                    retryNumber = httpParam.applyHealth.RetryNumber;
                    retryTimeSpan = TimeConversion(httpParam.applyHealth.RetryInterval, httpParam.applyHealth.RITypeName);
                }
                else if (httpParam.applyHealth.RetryPolicyName == "无限重试")
                {
                    //无线重试 上线100
                    retryNumber = 100;
                    retryTimeSpan = 5;
                }

                // 设置重试策略
                var retryPolicy = Policy.HandleResult<HttpResponseMessage>(reponse => !reponse.IsSuccessStatusCode)
                                .Or<TimeoutException>()
                                .WaitAndRetryAsync(retryNumber, retryAttempt => TimeSpan.FromSeconds(retryTimeSpan));

                //重试次数
                int retry = -1;
                var baseresponse = await retryPolicy.ExecuteAsync(async () =>
                {
                    retry++;
                    var request = new HttpRequestMessage(HttpMethod.Get, uriBuilder.ToString());

                    //var headerString = request.Headers
                    //                    .Select(h => $"{h.Key}: {h.Value}")
                    //                    .ToJsonString();
                    //httpParam.step1.ReponseHeaders = string.Join("\n", headerString);

                    //header
                    foreach (var item in httpParam.headers)
                    {
                        request.Headers.Add(item.Key, item.Value);
                    };

                    //body
                    request.Content = new StringContent(httpParam.body, Encoding.UTF8, "application/json");
                    return await httpClient.SendAsync(request);
                });

                var headerString = baseresponse.Headers
                     .Select(h => $"{h.Key}: {h.Value}")
                     .ToJsonString();
                httpParam.step1.ReponseHeaders = string.Join("\n", headerString);

                // 处理响应
                if (baseresponse.IsSuccessStatusCode)
                {
                    var responseJson = await baseresponse.Content.ReadAsStringAsync();
                    httpParam.Reponsedata.success = true;
                    httpParam.Reponsedata.code = "200";
                    httpParam.Reponsedata.message = $"请求成功";
                    httpParam.Reponsedata.exception = null;
                    httpParam.Reponsedata.data = JsonConvert.DeserializeObject<object>(responseJson);

                    #region 日志记录4 http返回 结束

                    httpParam.step1 = httpParam.step1;
                    httpParam.step1.EndTime = DateTime.Now;
                    httpParam.step1.StepStatus = true;
                    httpParam.step1.StepTime = (httpParam.step1.EndTime - httpParam.step1.StartTime).TotalMilliseconds.ToString();
                    httpParam.step1.ReponseCode = "200";
                    httpParam.step1.ReponseMessage = $"重试次数({retry})，重试间隔时间（{retryTimeSpan}秒）";
                    //httpParam.step1.ReponseHeaders = null;
                    httpParam.step1.ResponseBody = JsonConvert.SerializeObject(httpParam.Reponsedata);
                    await ApiSetLog(httpParam.step1, httpParam.applyHealth);

                    #endregion 日志记录4 http返回 结束

                    // 从登录响应中提取Cookie，并将其保存在本地
                    if (baseresponse.Headers.TryGetValues("Set-Cookie", out var cookieValues))
                    {
                        foreach (var cookieValue in cookieValues)
                        {
                            SetCookieHeaderValue.TryParse(cookieValue, out SetCookieHeaderValue mycokie);
                            //格式要是不正确直接不干了
                            if (mycokie != null)
                            {
                                var cookie = ToCookie(mycokie, baseAddress.Host.ToString());
                                _cookieContainer.Add(httpClient.BaseAddress, cookie);
                            }
                        }
                    }

                    return ApiResponse(httpParam.Reponsedata, httpParam.agentAPI);
                }
                else
                {
                    var responseJson = await baseresponse.Content.ReadAsStringAsync();

                    httpParam.Reponsedata.success = false;
                    httpParam.Reponsedata.code = "500";
                    httpParam.Reponsedata.message = $"请求失败";
                    httpParam.Reponsedata.exception = null;
                    httpParam.Reponsedata.data = JsonConvert.DeserializeObject<object>(responseJson);

                    #region 日志记录4 http返回 结束

                    httpParam.step1 = httpParam.step1;
                    httpParam.step1.EndTime = DateTime.Now;
                    httpParam.step1.StepStatus = false;
                    httpParam.step1.StepTime = (httpParam.step1.EndTime - httpParam.step1.StartTime).TotalMilliseconds.ToString();
                    httpParam.step1.ReponseCode = "500";
                    httpParam.step1.ReponseMessage = $"重试次数({retry})，重试间隔时间（{retryTimeSpan}秒）";
                    //httpParam.step1.ReponseHeaders = null;
                    httpParam.step1.ResponseBody = JsonConvert.SerializeObject(httpParam.Reponsedata);
                    await ApiSetLog(httpParam.step1, httpParam.applyHealth);

                    #endregion 日志记录4 http返回 结束

                    return ApiResponse(httpParam.Reponsedata, httpParam.agentAPI);
                }

                //response.EnsureSuccessStatusCode();
            }
        }

        /// <summary>
        /// http webservice请求
        /// </summary>
        /// <returns></returns>
        private async Task<IActionResult> CallWebService(HttpParamDto httpParam)
        {
            // 添加Cookie到HttpClient实例中
            HttpClientHandler httpClientHandler = new HttpClientHandler();
            httpClientHandler.CookieContainer = _cookieContainer;
            using (var httpClient = new HttpClient(httpClientHandler))
            {
                //链接超时策略
                httpClient.Timeout = HttpConnectionTimedOut(httpParam.applyHealth);
                var baseAddress = new Uri(httpParam.apiUrl);
                httpClient.BaseAddress = new Uri(baseAddress.Scheme + "://" + baseAddress.Host);

                httpClient.DefaultRequestHeaders.Add("SOAPAction", httpParam.SOAPAction);

                var requestBodyContent = new StringContent(httpParam.xmlRequest, Encoding.UTF8, "text/xml");

                //httpParam.applyHealth
                //从健康策略中读取重试次数 & 重试时间间隔
                int retryNumber = 0;
                int retryTimeSpan = 0;
                if (httpParam.applyHealth.RetryPolicyName == "限制重试次数")
                {
                    retryNumber = httpParam.applyHealth.RetryNumber;
                    retryTimeSpan = TimeConversion(httpParam.applyHealth.RetryInterval, httpParam.applyHealth.RITypeName);
                }
                else if (httpParam.applyHealth.RetryPolicyName == "无限重试")
                {
                    //无线重试 上线100
                    retryNumber = 100;
                    retryTimeSpan = 5;
                }

                // 设置重试策略
                var retryPolicy = Policy.HandleResult<HttpResponseMessage>(reponse => !reponse.IsSuccessStatusCode)
                                .Or<TimeoutException>()
                                .WaitAndRetryAsync(retryNumber, retryAttempt => TimeSpan.FromSeconds(retryTimeSpan));

                //重试次数
                int retry = -1;
                var baseresponse = await retryPolicy.ExecuteAsync(async () =>
                {
                    retry++;
                    // 发送 POST 请求
                    return await httpClient.PostAsync(httpParam.apiUrl, requestBodyContent);
                });

                var headerString = baseresponse.Headers
                    .Select(h => $"{h.Key}: {h.Value}")
                    .ToJsonString();
                httpParam.step1.ReponseHeaders = string.Join("\n", headerString);

                // 从登录响应中提取Cookie，并将其保存在本地
                if (baseresponse.Headers.TryGetValues("Set-Cookie", out var cookieValues))
                {
                    foreach (var cookieValue in cookieValues)
                    {
                        var issetCookie = SetCookieHeaderValue.TryParse(cookieValue, out SetCookieHeaderValue mycokie);
                        //格式要是不正确直接跳过
                        if (issetCookie)
                        {
                            var cookie = ToCookie(mycokie, baseAddress.Host.ToString());

                            Response.Cookies.Append(cookie.Name, cookie.ToString());
                            _cookieContainer.Add(httpClient.BaseAddress, cookie);
                        }
                    }
                }

                // 处理响应
                if (baseresponse.IsSuccessStatusCode)
                {
                    var responseJson = await baseresponse.Content.ReadAsStringAsync();
                    // 创建一个 XmlDocument 对象
                    var xmlDocument = new XmlDocument();
                    // 将 SOAP 响应字符串加载到 XmlDocument 中
                    xmlDocument.LoadXml(responseJson);

                    httpParam.Reponsedata.success = true;
                    httpParam.Reponsedata.code = "200";
                    httpParam.Reponsedata.message = $"请求成功";
                    httpParam.Reponsedata.exception = null;
                    //httpParam.Reponsedata.data = xmlDocument;
                    httpParam.Reponsedata.data = responseJson;

                    #region 日志记录4 http返回 结束

                    httpParam.step1 = httpParam.step1;
                    httpParam.step1.EndTime = DateTime.Now;
                    httpParam.step1.StepStatus = true;
                    httpParam.step1.StepTime = (httpParam.step1.EndTime - httpParam.step1.StartTime).TotalMilliseconds.ToString();
                    httpParam.step1.ReponseCode = "200";
                    httpParam.step1.ReponseMessage = $"重试次数({retry})，重试间隔时间（{retryTimeSpan}秒）";
                    httpParam.step1.ReponseHeaders = null;
                    httpParam.step1.ResponseBody = JsonConvert.SerializeObject(httpParam.Reponsedata);
                    await ApiSetLog(httpParam.step1, httpParam.applyHealth);

                    #endregion 日志记录4 http返回 结束

                    return ApiResponse(httpParam.Reponsedata, httpParam.agentAPI);
                }
                else
                {
                    var responseJson = await baseresponse.Content.ReadAsStringAsync();
                    // 创建一个 XmlDocument 对象
                    var xmlDocument = new XmlDocument();
                    // 将 SOAP 响应字符串加载到 XmlDocument 中
                    xmlDocument.LoadXml(responseJson);

                    httpParam.Reponsedata.success = false;
                    httpParam.Reponsedata.code = "500";
                    httpParam.Reponsedata.message = $"请求失败";
                    httpParam.Reponsedata.exception = null;
                    //httpParam.Reponsedata.data = xmlDocument;
                    httpParam.Reponsedata.data = responseJson;

                    #region 日志记录4 http返回 结束

                    httpParam.step1 = httpParam.step1;
                    httpParam.step1.EndTime = DateTime.Now;
                    httpParam.step1.StepStatus = false;
                    httpParam.step1.StepTime = (httpParam.step1.EndTime - httpParam.step1.StartTime).TotalMilliseconds.ToString();
                    httpParam.step1.ReponseCode = "500";
                    httpParam.step1.ReponseMessage = $"重试次数({retry})，重试间隔时间（{retryTimeSpan}秒）";
                    httpParam.step1.ReponseHeaders = null;
                    httpParam.step1.ResponseBody = JsonConvert.SerializeObject(httpParam.Reponsedata);
                    await ApiSetLog(httpParam.step1, httpParam.applyHealth);

                    #endregion 日志记录4 http返回 结束

                    return ApiResponse(httpParam.Reponsedata, httpParam.agentAPI);
                }
            }
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

        /// <summary>
        /// 获取流量策略、熔断策略 对应API缓存KEY
        /// </summary>
        /// <param name="guid"></param>
        /// <returns></returns>
        private string GetCasheStr(Guid guid, string type)
        {
            switch (type)
            {
                case "flowControl":
                    //流量策略缓存key
                    return "API_FC_Cache_" + guid.ToString();

                case "fusingMode":
                    //熔断策略缓存key
                    return "API_FM_Cache_" + guid.ToString();

                case "fusingModeLog":
                    //熔断策略缓存 实时记录日志
                    return "API_FM_Log_Cache_" + guid.ToString();

                default:
                    return "API_FC_Cache_Default";
            }
        }

        /// <summary>
        /// 连接中心 连接超时策略
        /// </summary>
        /// <returns></returns>
        private TimeSpan HttpConnectionTimedOut(ApplyHealthTestingListDto health)
        {
            //默认给一个小时
            TimeSpan span = TimeSpan.FromSeconds(3600);
            if (health != null)
            {
                span = TimeSpan.FromSeconds(TimeConversion(health.ConnectionTimeout, health.CtiTypeName));
            }
            return span;
        }

        /// <summary>
        /// 检查 API流量限制策略
        /// </summary>
        /// <returns></returns>
        private async Task<bool> CheckFlowControl(AgentAPIListDto agentAPI)
        {
            var flow = await _flowControlApp.GetFlowControlByAgentID(agentAPI.Id);
            if (flow != null)
            {
                //api流量限制（次数）
                int apilimit = flow.TrafficRestrictions;
                //缓存中的api请求次数
                ITypedCache<string, CacheItemDto> API_FC_Cache = _cacheManager.GetCache(GetCasheStr(agentAPI.Id, "flowControl")).AsTyped<string, CacheItemDto>();
                //key是否存在  是否需要设置过期时间
                var isSetting = API_FC_Cache.TryGetValue(GetCasheStr(agentAPI.Id, "flowControl"), out CacheItemDto cache_limit);

                // key不存在 新增一个 需要设置过期时间
                if (!isSetting)
                {
                    cache_limit = new CacheItemDto();
                    cache_limit.Key = agentAPI.Id.ToString();
                    cache_limit.value = 1;
                    ////单位时间使用秒作为最小单位
                    int unitTime = TimeConversion(flow.UnitTime, flow.UnitType);
                    //设置 过期时间
                    API_FC_Cache.DefaultSlidingExpireTime = TimeSpan.FromSeconds(unitTime);
                }
                else
                {
                    //已经存在就需要 增加api访问次数
                    cache_limit.value += 1;
                }
                API_FC_Cache.Set(GetCasheStr(agentAPI.Id, "flowControl"), cache_limit);

                //如果缓存中的api请求次数大于 设定的单位时间内的api流量限制次数 就返回
                if (cache_limit.value > apilimit)
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// 熔断模式校验
        /// </summary>
        /// <returns></returns>
        private bool CheckFusingModel(AgentAPIListDto agentAPI, ApplyHealthTestingListDto health)
        {
            if (health.FusingModeName == "无熔断")
            {
                return true;
            }
            else
            {
                //找到 api熔断模式 的缓存
                ITypedCache<string, CacheItemDto> API_FM_Cache = _cacheManager.GetCache(GetCasheStr(agentAPI.Id, "fusingMode")).AsTyped<string, CacheItemDto>();
                var isSetting = API_FM_Cache.TryGetValue(GetCasheStr(agentAPI.Id, "flowControl"), out CacheItemDto cache_limit);

                if (isSetting)
                {
                    //说明触发了熔断机制 返回false
                    return false;
                }
                return true;
            }
        }

        /// <summary>
        /// 熔断模式 加入异常记录缓存 & 使用API对应连接器配置的 熔断策略规则进行判断 是否需要熔断
        /// </summary>
        /// <param name="AgentAPI"></param>
        /// <param name="health"></param>
        /// <returns></returns>
        private async Task<bool> JudgeFusingModel(Guid AgentAPI, ApplyHealthTestingListDto health)
        {
            if (health == null)
            {
                return true;
            }
            if (health.FusingModeName == "无熔断")
            {
                return true;
            }
            var logkey = GetCasheStr(AgentAPI, "fusingModeLog");

            //当前异常记录
            JudgeFusingDto judgeFusing = new JudgeFusingDto();
            judgeFusing.HappenTime = DateTime.Now;
            judgeFusing.value = 1;

            //找到缓存中的异常记录
            ITypedCache<string, CacheItemDto> API_FC_Cache = _cacheManager.GetCache(logkey).AsTyped<string, CacheItemDto>();
            bool isSetting = true;
            var cache_limit = API_FC_Cache.Get(logkey, () =>
            {
                isSetting = false;
                List<JudgeFusingDto> judgeFusing_ = new List<JudgeFusingDto>();
                judgeFusing_.Add(judgeFusing);
                return new CacheItemDto
                {
                    Key = AgentAPI.ToString(),
                    value = 0,
                    fusingModeLog = judgeFusing_,
                };
            });

            //如果已经存在 就直接加入 异常记录
            if (isSetting)
            {
                cache_limit.fusingModeLog.Add(judgeFusing);
                API_FC_Cache.Set(logkey, cache_limit);
            }

            if (health.FusingModeName == "普通模式")
            {
                //现有异常次数超出了 策略限定的异常次数
                if (cache_limit.fusingModeLog.Count() > health.ExceptionsNumber)
                {
                    //启用熔断
                    startFusingModel(AgentAPI, health);
                    //清空当前熔断日志
                    API_FC_Cache.Remove(logkey);
                }
            }

            if (health.FusingModeName == "高级模式")
            {
                //第一条记录
                var firstLog = cache_limit.fusingModeLog[0];
                //采样截止时间
                DateTime endTime = firstLog.HappenTime.AddSeconds(TimeConversion(health.SamplingInterval, health.SITypeName));
                //计算公式
                //异常请求百分比 采样时间内异常次数除以吞吐量 ，得到失败率，若超过设置的异常百分比则触发熔断
                //采样时间区间 时间范围内内统计错误发生次数阈值，超过阈值则触发熔断
                //采样最小吞吐量	采样时间内请求次数的最小吞吐量
                //熔断持续时间	熔断后，多长时间恢复

                //采样时间区间 内的错误数量
                int SamplingIntervalErroCount = cache_limit.fusingModeLog.Where(t => t.HappenTime >= firstLog.HappenTime && t.HappenTime <= endTime).Count();

                //异常请求百分比 采样时间内异常次数除以吞吐量 ，得到失败率，若超过设置的异常百分比则触发熔断
                if (SamplingIntervalErroCount / health.MinimumThroughput * 100 > health.ExceptionsScale)
                {
                    //启用熔断
                    startFusingModel(AgentAPI, health);
                    //清空当前熔断日志
                    API_FC_Cache.Remove(logkey);
                }
                else
                {
                    //如果最后一条数据大于采样截止时间
                    //则 清除截止时间 之前的数据
                    if (cache_limit.fusingModeLog.Where(t => t.HappenTime > endTime).Count() > 0)
                    {
                        cache_limit.fusingModeLog = cache_limit.fusingModeLog.Where(t => t.HappenTime > endTime).ToList();
                        API_FC_Cache.Set(logkey, cache_limit);
                    }
                }
            }
            return true;
        }

        /// <summary>
        /// 启用熔断策略
        /// </summary>
        /// <returns></returns>
        private bool startFusingModel(Guid AgentAPI, ApplyHealthTestingListDto health)
        {
            //找到 api熔断模式 的缓存
            ITypedCache<string, CacheItemDto> API_FM_Cache = _cacheManager.GetCache(GetCasheStr(AgentAPI, "fusingMode")).AsTyped<string, CacheItemDto>();
            //找到这个api的熔断策略缓存  如果是刚刚新建的 说明没有触发熔断策略
            CacheItemDto cacheItem = new CacheItemDto();
            cacheItem.Key = AgentAPI.ToString();
            cacheItem.value = 0;
            API_FM_Cache.DefaultSlidingExpireTime = TimeSpan.FromSeconds(TimeConversion(health.ContinuousFusing, health.CFTypeName));
            API_FM_Cache.Set(GetCasheStr(AgentAPI, "flowControl"), cacheItem);
            return true;
        }

        /// <summary>
        /// 统一处理API日志插入
        /// </summary>
        /// <param name="step1"></param>
        /// <returns></returns>
        /// <returns></returns>
        private async Task<APIRequestRecord> ApiSetLog(APIRequestRecord step1, ApplyHealthTestingListDto? healthTesting = null)
        {
            try
            {
                //熔断策略 异常 记录缓存
                if (step1.StepType == "请求后台服务")
                {
                    //加入异常记录缓存 & 使用API对应连接器配置的 熔断策略规则进行判断 是否需要熔断
                    await JudgeFusingModel(step1.AgentAPIID, healthTesting);
                }
            }
            catch (Exception ex)
            {
                step1.Exception += $"[ApiSetLogCatch:{ex.Message}]";
                throw;
            }

            return await _apiLog.CreateAsync(step1);
        }

        /// <summary>
        /// 统一处理API返回数据
        /// </summary>
        /// <param name="reponseData"></param>
        /// <returns></returns>
        private IActionResult ApiResponse(ReponseData reponseData, AgentAPIListDto agentAPI)
        {
            if (reponseData.responseDataType == 0)
            {
                return Content(JsonConvert.SerializeObject(reponseData), "application/json");
            }
            else
            {
                return Content(JsonConvert.SerializeObject(reponseData.data), "application/json");
            }
        }

        /// <summary>
        /// 转换cokie
        /// </summary>
        /// <param name="setCookieHeaderValue"></param>
        /// <param name="domain"></param>
        /// <returns></returns>
        private Cookie ToCookie(SetCookieHeaderValue setCookieHeaderValue, string domain)
        {
            var cookie = new Cookie
            {
                Name = setCookieHeaderValue.Name.Value,
                Value = setCookieHeaderValue.Value.Value,
                Domain = domain,
                Path = setCookieHeaderValue.Path != null ? setCookieHeaderValue.Path.Value : null,
                Expires = setCookieHeaderValue.Expires.HasValue ? setCookieHeaderValue.Expires.Value.LocalDateTime : DateTime.MinValue,
                Secure = setCookieHeaderValue.Secure,
                HttpOnly = setCookieHeaderValue.HttpOnly
            };
            return cookie;
        }
    }
}
