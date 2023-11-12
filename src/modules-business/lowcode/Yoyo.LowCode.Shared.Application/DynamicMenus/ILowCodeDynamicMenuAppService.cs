using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.DynamicMenus.Dtos;

namespace Yoyo.LowCode.DynamicMenus
{
    /// <summary>
    /// 动态菜单应用层服务的接口方法
    ///</summary>
    public interface ILowCodeDynamicMenuAppService : IApplicationService
    {
        /// <summary>
		/// 获取动态菜单的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<LowCodeDynamicMenuListDto>> GetPaged(GetDynamicMenusInput input);

        /// <summary>
        /// 获取动态菜单的分页列表集合
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<LowCodeDynamicMenuListDto>> GetList();

        /// <summary>
        /// 通过指定id获取动态菜单ListDto信息
        /// </summary>
        Task<LowCodeDynamicMenuListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 返回实体动态菜单的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetDynamicMenuForEditOutput> GetForEdit(NullableIdDto<Guid> input);

        /// <summary>
        /// 添加或者修改动态菜单的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateDynamicMenuInput input);

        /// <summary>
        /// 删除动态菜单
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除动态菜单
        /// </summary>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 重置
        /// </summary>
        /// <returns></returns>
        Task Reset();

        //// custom codes

        //// custom codes end
    }
}
