using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.DbLinks.Dtos;

namespace Yoyo.LowCode.DbLinks
{
    /// <summary>
    /// 数据连接应用层服务的接口方法
    ///</summary>
    public interface IDbLinkAppService : IApplicationService
    {
        /// <summary>
		/// 获取数据连接的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<DbLinkListDto>> GetPaged(GetDbLinksInput input);

        /// <summary>
        /// 通过指定id获取数据连接ListDto信息
        /// </summary>
        Task<DbLinkListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 返回实体数据连接的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetDbLinkForEditOutput> GetForEdit(NullableIdDto<Guid> input);

        /// <summary>
        /// 添加或者修改数据连接的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateDbLinkInput input);

        /// <summary>
        /// 删除数据连接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除数据连接
        /// </summary>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 测试连接
        /// </summary>
        /// <param name="input">实体对象</param>
        /// <returns></returns>
        Task TestDbConnection(DbLinkListDto input);

        /// <summary>
        /// 获取数据库下拉列表
        /// </summary>
        /// <returns></returns>
        Task<List<DbLinkSelectorOutput>> GetDbSelector();

        //// custom codes

        //// custom codes end
    }
}
