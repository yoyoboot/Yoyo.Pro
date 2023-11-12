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
using Yoyo.LowCode.DataDictionarys.DomainService;
using Yoyo.LowCode.DataDictionarys.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.LowCode.DataDictionarys
{
    /// <summary>
    /// 数据字典
    /// </summary>
    //[AbpAuthorize]
    public class LowCodeDataDictionaryAppService : LowCodeSharedAppServiceBase, ILowCodeDataDictionaryAppService
    {
        #region Private Fields

        private readonly IDataDictionarysManager _dataDictionarysManager;
        private readonly IDataDictionarysValueManager _dataDictionarysValueManager;

        #endregion Private Fields

        #region Public Constructors

        public LowCodeDataDictionaryAppService(IDataDictionarysManager dataDictionarys, IDataDictionarysValueManager dataDictionarysValue)
        {
            _dataDictionarysManager = dataDictionarys;
            _dataDictionarysValueManager = dataDictionarysValue;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <summary>
        ///     批量删除DataDictionaryPage的方法
        /// </summary>
        public async Task BatchDelete(List<Guid> input)
        {
            await _dataDictionarysManager.BatchDelete(input);
        }

        /// <summary>
        ///     添加或者修改数据字典的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdate(CreateOrUpdateDictonary input)
        {
            if (input.LowCodeDataDictionaryPage.Id.HasValue)
            {
                await Update(input.LowCodeDataDictionaryPage);
            }
            else
            {
                await Create(input.LowCodeDataDictionaryPage);
            }
        }

        /// <summary>
        ///     删除数据字典信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _dataDictionarysManager.DeleteAsync(input.Id);
        }

        /// <summary>
        /// 通过指定id获取数据字典DataDictionaryListDto
        /// </summary>
        public async Task<LowCodeDataDictionaryListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _dataDictionarysManager.EntityRepo.GetAsync(input.Id);

            var dto = ObjectMapper.Map<LowCodeDataDictionaryListDto>(entity);
            return dto;
        }

        /// <summary>
        ///     获取编辑 数据字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetLowCodeDataDictionaryForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetLowCodeDataDictionaryForEditOutput();
            LowCodeDataDictionaryEditDto editDto;

            if (input.Id.HasValue)
            {
                var entity = await _dataDictionarysManager.QueryAsNoTracking
                    .Include(x => x.BaseDictionaryValues)
                    .FirstOrDefaultAsync(x => x.Id == input.Id.Value);
                //var entitys = await _dataDictionarysManager.EntityRepo.GetAsync(input.Id.Value);
                editDto = ObjectMapper.Map<LowCodeDataDictionaryEditDto>(entity);
            }
            else
            {
                editDto = new LowCodeDataDictionaryEditDto();
            }

            output.LowCodeDataDictionary = editDto;
            return output;
        }

        /// <summary>
        ///获取数据字典ListDto信息
        /// </summary>
        public async Task<List<LowCodeDataDictionaryListDto>> GetList(string filterText)
        {
            var query = _dataDictionarysManager.Query
                .WhereIf(!filterText.IsNullOrWhiteSpace(), a =>
                    a.Name.Contains(filterText) ||
                    a.Description.Contains(filterText)
                );

            var list = await query.ToListAsync();
            return ObjectMapper.Map<List<LowCodeDataDictionaryListDto>>(list);
        }

        /// <summary>
        ///     获取数据字典的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<LowCodeDataDictionaryListDto>> GetPaged(DataDictionarysInput input)
        {


            var query = _dataDictionarysManager.Query
                .Include(x => x.BaseDictionaryValues)
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>
                    a.Code.Contains(input.FilterText) ||
                    a.Name.Contains(input.FilterText) ||
                    a.Description.Contains(input.FilterText)
                );

            var count = await query.CountAsync();

            var dataDictionaryList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var dataDictionaryListDto = ObjectMapper.Map<List<LowCodeDataDictionaryListDto>>(dataDictionaryList);

            return new PagedResultDto<LowCodeDataDictionaryListDto>(count, dataDictionaryListDto);
        }
        /// <summary>
        /// 获取数据字典下拉列表
        /// </summary>
        /// <returns></returns>
        public async Task<List<LowCodeDataDictionaryListDto>> SelectDataDictionary()
        {
            var entity = await _dataDictionarysManager.QueryAsNoTracking.ToListAsync();

            var dataDictionaryList = ObjectMapper.Map<List<LowCodeDataDictionaryListDto>>(entity);

            return dataDictionaryList;

        }

        /// <summary>
        /// 通过id获取字典内部的数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        public async Task<List<SelectDictionaryValue>> SelectDataDictionaryvalue(EntityDto<Guid> input)
        {
            var entity = await _dataDictionarysManager.QueryAsNoTracking
                    .Include(x => x.BaseDictionaryValues)
                    .OrderBy(x => x.OrderNo)
                    .FirstOrDefaultAsync(x => x.Id == input.Id);
            var dto = ObjectMapper.Map<List<SelectDictionaryValue>>(entity?.BaseDictionaryValues);

            return dto;

        }

        #endregion Public Methods

        #region Protected Methods

        /// <summary>
        ///     新增数据字典
        /// </summary>
        protected virtual async Task<LowCodeDataDictionaryEditDto> Create(LowCodeDataDictionaryEditDto input)
        {
            var deduplicationList = await _dataDictionarysManager.QueryAsNoTracking.FirstOrDefaultAsync(x => x.Code == input.Code);
            if (deduplicationList != null)
            {
                throw new Abp.UI.UserFriendlyException(L($"字典代码{input.Code}已被使用，请重新输入代码"));
            }
            var dictionaryList = input.BaseDictionaryValues;
            if (dictionaryList.GroupBy(i => i.Code).Where(g => g.Count() > 1).Count() > 0)
            {
                throw new Abp.UI.UserFriendlyException(L($"{input.Name}字典中存在重复代码，请重新输入代码"));
            }
            var entity = ObjectMapper.Map<BaseDictionaryType>(input);
            entity = await _dataDictionarysManager.CreateAsync(entity);

            var dto = ObjectMapper.Map<LowCodeDataDictionaryEditDto>(entity);
            return dto;
        }

        protected async Task<List<BaseDictionaryValue>> ProcessEntrys(BaseDictionaryType entity, LowCodeDataDictionaryEditDto input)
        {
            var entrys = new List<BaseDictionaryValue>();

            var oldData = entity.BaseDictionaryValues;
            var newData = ObjectMapper.Map<List<BaseDictionaryValue>>(input.BaseDictionaryValues);
            if (oldData == null)
            {
                oldData = new List<BaseDictionaryValue>();
            }

            // 删除项操作
            var deleted = oldData.Except(newData);
            foreach (var item in deleted)
            {
                await _dataDictionarysValueManager.Delete(item);
            }

            // 更新项
            var updated = newData.Intersect(oldData).ToList();
            foreach (var item in updated)
            {
                var oldItem = oldData.Find(x => x.Id == item.Id);
                ObjectMapper.Map(ObjectMapper.Map<BaseDictionaryValueEditDto>(item), oldItem);
                await _dataDictionarysValueManager.Update(oldItem);

                entrys.Add(oldItem);
            }

            // 新增项
            var inserted = newData.Where(x => x.Id == default).ToList();
            foreach (var item in inserted)
            {
                item.Id = GuidGenerator.Create();
                item.BaseDictionaryType = entity;
                item.BaseDictionaryTypeId = entity.Id;
                await _dataDictionarysValueManager.Create(item);
                entrys.Add(item);
            }

            return entrys;
        }

        /// <summary>
        ///     编辑数据字典
        /// </summary>
        protected virtual async Task Update(LowCodeDataDictionaryEditDto input)
        {
            var deduplicationList = await _dataDictionarysManager.QueryAsNoTracking.ToListAsync();

            if (deduplicationList.GroupBy(i => i.Code).Where(g => g.Count() > 1).Count() > 0)
            {
                throw new Abp.UI.UserFriendlyException(L($"字典代码{input.Code}已被使用，请重新输入代码"));
            }
            var dictionaryList = input.BaseDictionaryValues;
            if (dictionaryList.GroupBy(i => i.Code).Where(g => g.Count() > 1).Count() > 0)
            {
                throw new Abp.UI.UserFriendlyException(L($"{input.Name}字典中存在重复代码，请重新输入代码"));
            }
            var entity = await _dataDictionarysManager.QueryAsNoTracking
                .Include(x => x.BaseDictionaryValues)
                .FirstOrDefaultAsync(x => x.Id == input.Id.Value);

            var entrys = await ProcessEntrys(entity, input);

            ObjectMapper.Map(input, entity);

            entity.BaseDictionaryValues = entrys;
            await _dataDictionarysManager.UpdateAsync(entity);
        }

        [HttpGet]
        public object Json()
        {
            int[] y = new int[] { 120, 200, 150, 80, 70, 110, 130 };
            string[] b = new string[] { "Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun" };
            var options = new
            {
                xAxis = b,
                series = y

            };
            return options;
        }
    }
}
#endregion
