// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.TemplateData.DomainService;
using Yoyo.LowCode.TemplateData.Dtos;
using Yoyo.LowCode.TemplateData.Entity;

namespace Yoyo.LowCode.TemplateData
{
    /// <summary>
    /// 表单模板
    /// </summary>
    public class TemplateDataAppService : LowCodeSharedAppServiceBase, ITemplateDataAppService
    {
        private readonly IBaseTemplateDataManager _baseTemplateDataManager;

        /// <summary>
        ///     构造函数
        /// </summary>
        public TemplateDataAppService(IBaseTemplateDataManager baseTemplateDataManager)
        {
            _baseTemplateDataManager = baseTemplateDataManager;
        }

        /// <summary>
        /// 获取表单模板集合
        /// </summary>
        /// <returns></returns>
        public async Task<List<BaseTemplateDataListDto>> BaseTemplateDataGetList()
        {
            var list = await _baseTemplateDataManager.QueryAsNoTracking
                .Select((e) => new BaseTemplateData
                {
                    Id = e.Id,
                    CreationTime = e.CreationTime,
                    ConcurrencyToken = e.ConcurrencyToken,
                    CreatorUserId = e.CreatorUserId,
                    CreatorUserName = e.CreatorUserName,
                    DeleterUserId = e.DeleterUserId,
                    DeleterUserName = e.DeleterUserName,
                    DeletionTime = e.DeletionTime,
                    IsDeleted = e.IsDeleted,
                    LastModificationTime = e.LastModificationTime,
                    LastModifierUserId = e.LastModifierUserId,
                    LastModifierUserName = e.LastModifierUserName,
                    TemplateName = e.TemplateName,
                    TableTemplateJson = e.TableTemplateJson,
                    ColumnTemplateJson = e.ColumnTemplateJson,
                    FormTemplateJson = "",
                    Description = e.Description,
                    ImagesUrl = e.ImagesUrl
                })
                .ToListAsync();
            return ObjectMapper.Map<List<BaseTemplateDataListDto>>(list);
        }

        /// <summary>
        /// 通过Id获取指定表单模板
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<BaseTemplateDataListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _baseTemplateDataManager.EntityRepo.GetAsync(input.Id);

            var dto = ObjectMapper.Map<BaseTemplateDataListDto>(entity);
            return dto;
        }

        Task<List<BaseTemplateDataListDto>> ITemplateDataAppService.BaseTemplateDataGetList()
        {
            throw new NotImplementedException();
        }
    }
}
