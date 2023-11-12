// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using Abp.Application.Services.Dto;
using Abp.Linq.Extensions;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.ComField.Dtos;
using Yoyo.LowCode.Databases.DomainService;
using Yoyo.LowCode.Databases.Entity;

namespace Yoyo.LowCode.ComField
{
    /// <summary>
    /// 常用字段
    /// </summary>
    public class ComFieldAppService : LowCodeSharedAppServiceBase, IComFieldAppService
    {
        private readonly IComFieldManager _fieldManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="fieldManager"></param>
        public ComFieldAppService(IComFieldManager fieldManager)
        {
            _fieldManager = fieldManager;
        }

        /// <summary>
        /// 添加或修改常用字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdate(ComFieldForCreateOrUpdate input)
        {
            if (input.ComField.Id.HasValue)
            {
                await Update(input.ComField);
            }
            else
            {
                await Create(input.ComField);
            }
        }

        /// <summary>
        /// 添加常用字段
        /// </summary>
        /// <param name="comField"></param>
        /// <returns></returns>
        protected virtual async Task<ComFieldEditDto> Create(ComFieldEditDto comField)
        {
            var entity = ObjectMapper.Map<BaseComFields>(comField);

            entity = await _fieldManager.CreateAsync(entity);

            var dto = ObjectMapper.Map<ComFieldEditDto>(entity);

            return dto;
        }

        /// <summary>
        /// 修改常用字段
        /// </summary>
        /// <param name="comField"></param>
        /// <returns></returns>
        private async Task Update(ComFieldEditDto comField)
        {
            if (comField.Id != null)
            {
                var entity = await _fieldManager.EntityRepo.GetAsync(comField.Id.Value);
                ObjectMapper.Map(comField, entity);
                await _fieldManager.UpdateAsync(entity);
            }
        }

        /// <summary>
        /// 删除常用字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _fieldManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 获取编辑的常用字段
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<ComFieldForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new ComFieldForEditOutput();
            ComFieldEditDto comFieldEdit;
            if (input.Id.HasValue)
            {
                var entity = await _fieldManager.EntityRepo.GetAsync(input.Id.Value);
                comFieldEdit = ObjectMapper.Map<ComFieldEditDto>(entity);
            }
            else
            {
                comFieldEdit = new ComFieldEditDto();
            }
            output.ComField = comFieldEdit;
            return output;
        }

        public async Task<PagedResultDto<ComFieldListDto>> GetPaged(ComFieldInput input)
        {
            var query = _fieldManager.Query;
            var count = await query.CountAsync();
            var baseComFieldList = await query.OrderBy(input.Sorting).AsNoTracking().PageBy(input).ToListAsync();
            var baseComFieldListDtos = ObjectMapper.Map<List<ComFieldListDto>>(baseComFieldList);
            return new PagedResultDto<ComFieldListDto>(count, baseComFieldListDtos);
        }

        /// <summary>
        /// 获取常用字段下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<ComFieldSelectorOutput>> GetComFieldSelector()
        {
            var comField = await _fieldManager.QueryAsNoTracking.OrderBy(x => x.SortCode).ToListAsync();

            var list = ObjectMapper.Map<List<ComFieldSelectorOutput>>(comField);
            return list;
        }
    }
}
