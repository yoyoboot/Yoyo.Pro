using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Yoyo.LowCode.Databases.DomainService;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.LowCode.DbLinks.Dtos;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.Extension;

namespace Yoyo.LowCode.DbLinks
{
    /// <summary>
    /// 数据连接应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize]
    public class DbLinkAppService : LowCodeSharedAppServiceBase, IDbLinkAppService
    {
        private readonly IDbLinkManager _dbLinkManager;

        /// <summary>
        ///     构造函数
        /// </summary>
        public DbLinkAppService(IDbLinkManager dbLinkManager)
        {
            _dbLinkManager = dbLinkManager;
        }

        /// <summary>
        ///     获取数据连接的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(DbLinkPermissions.BaseDbLink_Query)]
        public async Task<PagedResultDto<DbLinkListDto>> GetPaged(GetDbLinksInput input)
        {
            var query = _dbLinkManager.Query.WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>

                //模糊搜索连接名称
                a.FullName.Contains(input.FilterText) ||

                //模糊搜索主机名称
                a.Host.Contains(input.FilterText) ||

                //模糊搜索用户名
                a.UserName.Contains(input.FilterText) ||

                //模糊搜索服务名
                a.ServiceName.Contains(input.FilterText) ||

                //模糊搜索描述
                a.Description.Contains(input.FilterText)
            );

            var count = await query.CountAsync();

            var baseDbLinkList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var baseDbLinkListDtos = ObjectMapper.Map<List<DbLinkListDto>>(baseDbLinkList);

            foreach (var item in baseDbLinkListDtos)
            {
                item.ConnectionString = DataHelper.ToConnectionString(item.DbType, item.Host, item.Port, item.UserName, item.Password, item.ServiceName);
            }

            return new PagedResultDto<DbLinkListDto>(count, baseDbLinkListDtos);
        }

        /// <summary>
        ///     通过指定id获取BaseDbLinkListDto信息
        /// </summary>
        //[AbpAuthorize(DbLinkPermissions.BaseDbLink_Query)]
        public async Task<DbLinkListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _dbLinkManager.EntityRepo.GetAsync(input.Id);

            var dto = ObjectMapper.Map<DbLinkListDto>(entity);
            return dto;
        }

        /// <summary>
        ///     获取编辑 数据连接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(DbLinkPermissions.BaseDbLink_Create, DbLinkPermissions.BaseDbLink_Edit)]
        public async Task<GetDbLinkForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetDbLinkForEditOutput();
            DbLinkEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _dbLinkManager.EntityRepo.GetAsync(input.Id.Value);
                editDto = ObjectMapper.Map<DbLinkEditDto>(entity);
            }
            else
            {
                editDto = new DbLinkEditDto();
            }

            output.DbLink = editDto;
            return output;
        }

        /// <summary>
        ///     添加或者修改数据连接的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(DbLinkPermissions.BaseDbLink_Create, DbLinkPermissions.BaseDbLink_Edit)]
        public async Task CreateOrUpdate(CreateOrUpdateDbLinkInput input)
        {
            if (input.DbLink.Id.HasValue)
            {
                await Update(input.DbLink);
            }
            else
            {
                await Create(input.DbLink);
            }
        }

        /// <summary>
        ///     删除数据连接信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        //[AbpAuthorize(DbLinkPermissions.BaseDbLink_Delete)]
        public async Task Delete(EntityDto<Guid> input)
        {
            await _dbLinkManager.DeleteAsync(input.Id);
        }

        /// <summary>
        ///     批量删除BaseDbLink的方法
        /// </summary>
        //[AbpAuthorize(DbLinkPermissions.BaseDbLink_BatchDelete)]
        public async Task BatchDelete(List<Guid> input)
        {
            await _dbLinkManager.BatchDelete(input);
        }

        /// <summary>
        ///     测试连接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task TestDbConnection(DbLinkListDto input)
        {
            await _dbLinkManager.IsConnection(ObjectMapper.Map<BaseDbLink>(input));
        }

        /// <summary>
        ///     获取数据库下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<DbLinkSelectorOutput>> GetDbSelector()
        {
            var dbLinks = await _dbLinkManager.QueryAsNoTracking.OrderBy(x => x.SortCode).ToListAsync();

            var ret = ObjectMapper.Map<List<DbLinkSelectorOutput>>(dbLinks);

            var defaultDbLink = _dbLinkManager.GetDefaultDbLink();

            ret.Add(new DbLinkSelectorOutput
            {
                DbType = defaultDbLink.DbType,
                FullName = defaultDbLink.FullName,
                IsDefault = true
            });

            return ret;
        }

        /// <summary>
        ///     新增数据连接
        /// </summary>
        //[AbpAuthorize(DbLinkPermissions.BaseDbLink_Create)]
        protected virtual async Task<DbLinkEditDto> Create(DbLinkEditDto input)
        {
            var entity = ObjectMapper.Map<BaseDbLink>(input);
            //调用领域服务
            entity = await _dbLinkManager.CreateAsync(entity);

            var dto = ObjectMapper.Map<DbLinkEditDto>(entity);
            return dto;
        }

        /// <summary>
        ///     编辑数据连接
        /// </summary>
        //[AbpAuthorize(DbLinkPermissions.BaseDbLink_Edit)]
        protected virtual async Task Update(DbLinkEditDto input)
        {
            if (input.Id != null)
            {
                var entity = await _dbLinkManager.EntityRepo.GetAsync(input.Id.Value);
                //  input.MapTo(entity);
                //将input属性的值赋值到entity中
                ObjectMapper.Map(input, entity);
                await _dbLinkManager.UpdateAsync(entity);
            }
        }

        //// custom codes

        //// custom codes end
    }
}
