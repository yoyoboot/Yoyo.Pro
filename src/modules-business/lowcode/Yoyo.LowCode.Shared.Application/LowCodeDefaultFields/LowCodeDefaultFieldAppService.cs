// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Abp.UI;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.LowCodeDefaultFields.Dtos;
using Yoyo.LowCode.LowCodeViewModels;
using Yoyo.LowCode.LowCodeViewModels.DomainService;

namespace Yoyo.LowCode.LowCodeDefaultFields
{
    public class LowCodeDefaultFieldAppService : LowCodeSharedAppServiceBase, ILowCodeDefaultFieldAppService
    {
        private readonly IDefaultFieldManager _defaultFieldManager;

        public LowCodeDefaultFieldAppService(IDefaultFieldManager defaultFieldManager)
        {
            _defaultFieldManager = defaultFieldManager;
        }

        /// <summary>
        /// 获取默认字段的分页列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<LowCodeDefaultFieldListDto>> GetPaged(LowCodeDefaultFieldInput input)
        {
            var query = _defaultFieldManager.Query
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(),
                a => a.FieldName.Contains(input.FilterText) ||
                a.FieldDesc.Contains(input.FilterText));
            var count = await query.CountAsync();
            var FieldList = await query.OrderBy(input.Sorting).AsNoTracking().PageBy(input).ToListAsync();
            var dto = ObjectMapper.Map<List<LowCodeDefaultFieldListDto>>(FieldList);
            return new PagedResultDto<LowCodeDefaultFieldListDto>(count, dto);
        }

        /// <summary>
        /// 添加或修改默认字段
        /// </summary>
        /// <param name="defaultField"></param>
        /// <returns></returns>
        public async Task CreateOrUpdate(LowCodeDefaultFieldCreateOrUpdate defaultField)
        {
            if (defaultField.LowCodeDefaultField.Id.HasValue)
            {
                await Update(defaultField.LowCodeDefaultField);
            }
            else
            {
                await Create(defaultField.LowCodeDefaultField);
            }
        }

        /// <summary>
        /// 添加默认字段
        /// </summary>
        /// <param name="fieldEditDto"></param>
        /// <returns></returns>
        protected virtual async Task<LowCodeDefaultFieldEditDto> Create(LowCodeDefaultFieldEditDto fieldEditDto)
        {
            await FieldIsExists(fieldEditDto.FieldName);

            var entity = ObjectMapper.Map<LowCodeDefaultField>(fieldEditDto);

            entity = await _defaultFieldManager.CreateAsync(entity);

            var dto = ObjectMapper.Map<LowCodeDefaultFieldEditDto>(entity);

            return dto;
        }

        /// <summary>
        /// 修改默认字段
        /// </summary>
        /// <param name="fieldEditDto"></param>
        /// <returns></returns>
        protected async Task Update(LowCodeDefaultFieldEditDto fieldEditDto)
        {
            if (fieldEditDto.Id != null)
            {
                await FieldIsExists(fieldEditDto.FieldName);
                var entity = await _defaultFieldManager.EntityRepo.GetAsync(fieldEditDto.Id.Value);
                ObjectMapper.Map(fieldEditDto, entity);
                await _defaultFieldManager.UpdateAsync(entity);
            }
        }

        /// <summary>
        /// 判断字段是否已存在
        /// </summary>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        /// <exception cref="UserFriendlyException"></exception>
        private async Task FieldIsExists(string fieldName)
        {
            var entity = await _defaultFieldManager.QueryAsNoTracking.FirstOrDefaultAsync(x => x.FieldName == fieldName);
            if (entity != null)
            {
                throw new UserFriendlyException(L("Error"), "该字段已存在，不可重复添加");
            }
        }

        /// <summary>
        /// 删除默认字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _defaultFieldManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 批量删除
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task BatchDelete(List<Guid> input)
        {
            await _defaultFieldManager.BatchDelete(input);
        }

        /// <summary>
        /// 获取编辑的默认字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<LowCodeDefaultFieldForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new LowCodeDefaultFieldForEditOutput();
            LowCodeDefaultFieldEditDto comFieldEdit;
            if (input.Id.HasValue)
            {
                var entity = await _defaultFieldManager.EntityRepo.GetAsync(input.Id.Value);
                comFieldEdit = ObjectMapper.Map<LowCodeDefaultFieldEditDto>(entity);
            }
            else
            {
                comFieldEdit = new LowCodeDefaultFieldEditDto();
            }
            output.LowCodeDefaultField = comFieldEdit;
            return output;
        }
    }
}
