// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Abp.Application.Services;
using Abp.Application.Services.Dto;
using Yoyo.LowCode.DataDictionarys.Dtos;

namespace Yoyo.LowCode.DataDictionarys
{
    /// <summary>
    /// 数据字典应用层服务的接口方法
    ///</summary>
    public interface ILowCodeDataDictionaryAppService : IApplicationService
    {
        /// <summary>
		/// 获取数据字典的分页列表集合
		///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<PagedResultDto<LowCodeDataDictionaryListDto>> GetPaged(DataDictionarysInput input);

        /// <summary>
        /// 获取数据字典的分页列表集合
        ///</summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<LowCodeDataDictionaryListDto>> GetList(string FilterText);

        /// <summary>
        /// 通过指定id获取数据字典ListDto信息
        /// </summary>
        Task<LowCodeDataDictionaryListDto> GetById(EntityDto<Guid> input);

        /// <summary>
        /// 返回实体数据字典的EditDto
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<GetLowCodeDataDictionaryForEditOutput> GetForEdit(NullableIdDto<Guid> input);

        /// <summary>
        /// 添加或者修改数据字典的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task CreateOrUpdate(CreateOrUpdateDictonary input);

        /// <summary>
        /// 删除数据字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task Delete(EntityDto<Guid> input);

        /// <summary>
        /// 批量删除数据字典
        /// </summary>
        Task BatchDelete(List<Guid> input);

        /// <summary>
        /// 获取字典下拉列表
        /// </summary>
        /// <returns></returns>
        Task<List<LowCodeDataDictionaryListDto>> SelectDataDictionary();

        /// <summary>
        /// 听过Id获取字典内部的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        Task<List<SelectDictionaryValue>> SelectDataDictionaryvalue(EntityDto<Guid> input);

        //// custom codes

        //// custom codes end
    }
}
