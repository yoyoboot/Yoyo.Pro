using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.CustomPages.Dtos;
using Yoyo.LowCode.Dtos;

namespace Yoyo.LowCode.CustomPages
{
    /// <summary>
    /// 功能设计应用层服务的接口方法
    ///</summary>
    public interface ICustomPageAppService : IApplicationService
    {
        /// <summary>
        /// 获取功能设计的分页列表集合
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<CustomPageListDto>> GetPaged(GetCustomPagesInput input);

        /// <summary>
        /// 获取功能设计的分页列表集合
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<CustomPageListDto>> GetList(string FilterText);

        /// <summary>
        /// 用户Ndo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<NdoStringDto>> GetUserNdo(LowCodeGetQueryFilterInput input);

        /// <summary>
        /// 角色Ndo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<NdoStringDto>> GetRoleNdo(LowCodeGetQueryFilterInput input);

        /// <summary>
        /// 通过指定id获取功能设计ListDto信息
        /// </summary>
        Task<CustomPageListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 返回实体功能设计的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetCustomPageForEditOutput> GetForEdit(NullableIdDto<Guid> input);

        /// <summary>
        /// 添加或者修改功能设计的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(SaveCustomPagesAndAutomatic input);

        /// <summary>
        /// 删除功能设计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除功能设计
        /// </summary>
        Task BatchDelete(List<Guid> input);

        Task<List<LowCodePpcDataPointDto>> GetPpcDataPointDto(string id);

        Task<TrasferimentoEditDto> TableCreationPrompt(CreateOrUpdateCustomPageInput input);

        /// <summary>
        /// 自动建表
        /// </summary>
        /// <param name="tableDto"></param>
        /// <returns></returns>
        Task AutoCreateTable(TrasferimentoEditDto tableDto);

        //// custom codes

        //// custom codes end
        ///
    }
}
