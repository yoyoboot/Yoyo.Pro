using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading.Tasks;
using System.Transactions;
using Abp.Application.Services.Dto;
using Abp.Authorization;
using Abp.Domain.Uow;
using Abp.Extensions;
using Abp.Linq.Extensions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.AutomaticTables.DomainService;
using Yoyo.LowCode.AutomaticTables.Dtos;
using Yoyo.LowCode.CustomPages.DomainService;
using Yoyo.LowCode.CustomPages.Dtos;
using Yoyo.LowCode.Dtos;
using Yoyo.LowCode.LowCodeViewModels.DomainService;
using Yoyo.LowCode.LowCodeViewModels.Dtos;
using Yoyo.LowCode.RoleManagement;
using Yoyo.LowCode.UserManagement;

namespace Yoyo.LowCode.CustomPages
{
    /// <summary>
    ///     功能设计应用层服务的接口实现方法
    /// </summary>
    [AbpAuthorize]
    public class CustomPageAppService : LowCodeSharedAppServiceBase, ICustomPageAppService
    {
        private readonly ICustomPageManager _customPageManager;

        private readonly LowCodeUserManager _userManager;

        private readonly LowCodeRoleManager _roleManager;

        private readonly IAutomaticTablesManager _automaticTablesManager;

        private readonly IUnitOfWorkManager _unitOfWorkManager;

        private readonly ILowCodeModelRelationManager _ModelRelationManager;

        /// <summary>
        /// 构造函数
        /// </summary>
        public CustomPageAppService(ICustomPageManager customPageManager,
            LowCodeUserManager userManager,
            LowCodeRoleManager roleManager,
            IAutomaticTablesManager automaticTablesManager,
            IUnitOfWorkManager unitOfWorkManager,
            ILowCodeModelRelationManager modelRelationManager)
        {
            _customPageManager = customPageManager;
            _userManager = userManager;
            _roleManager = roleManager;
            _automaticTablesManager = automaticTablesManager;
            _unitOfWorkManager = unitOfWorkManager;
            _ModelRelationManager = modelRelationManager;
        }

        /// <summary>
        /// 获取功能设计的分页列表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<PagedResultDto<CustomPageListDto>> GetPaged(GetCustomPagesInput input)
        {
            var query = _customPageManager.Query
                .WhereIf(!input.FilterText.IsNullOrWhiteSpace(), a =>
                    a.ViewName.Contains(input.FilterText) ||
                    a.ViewDesc.Contains(input.FilterText)
                );

            var count = await query.CountAsync();

            var baseCustomPageList = await query
                .OrderBy(input.Sorting).AsNoTracking()
                .PageBy(input)
                .ToListAsync();

            var baseCustomPageListDtos = ObjectMapper.Map<List<CustomPageListDto>>(baseCustomPageList);

            return new PagedResultDto<CustomPageListDto>(count, baseCustomPageListDtos);
        }

        public async Task<List<CustomPageListDto>> GetList(string filterText)
        {
            var query = _customPageManager.Query
                .WhereIf(!filterText.IsNullOrWhiteSpace(), a =>
                    a.ViewName.Contains(filterText) ||
                    a.ViewDesc.Contains(filterText)
                );

            var list = await query.ToListAsync();
            return ObjectMapper.Map<List<CustomPageListDto>>(list);
        }

        /// <summary>
        /// 通过指定id获取BaseCustomPageListDto信息
        /// </summary>
        public async Task<CustomPageListDto> GetById(EntityDto<Guid> input)
        {
            var entity = await _customPageManager
                .QueryAsNoTracking
                .FirstOrDefaultAsync(x => x.Id == input.Id);

            var dto = ObjectMapper.Map<CustomPageListDto>(entity);
            return dto;
        }

        /// <summary>
        ///     获取编辑 功能设计
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task<GetCustomPageForEditOutput> GetForEdit(NullableIdDto<Guid> input)
        {
            var output = new GetCustomPageForEditOutput();
            CustomPageEditDto editDto;
            List<LowCodeModelRelationEditDto> relationEditDtoList;

            if (input.Id.HasValue)
            {
                var entity = await _customPageManager.QueryAsNoTracking.FirstOrDefaultAsync(x => x.Id == input.Id.Value);

                var relationList = await _ModelRelationManager.
                    QueryAsNoTracking.Where(x => x.BaseCustomPageId == input.Id.Value).ToListAsync();
                editDto = ObjectMapper.Map<CustomPageEditDto>(entity);
                relationEditDtoList = ObjectMapper.Map<List<LowCodeModelRelationEditDto>>(relationList);
            }
            else
            {
                editDto = new CustomPageEditDto();
                relationEditDtoList = new List<LowCodeModelRelationEditDto>();
            }

            output.CustomPage = editDto;
            output.CodeModelRelationList = relationEditDtoList;
            return output;
        }

        /// <summary>
        ///     添加或者修改功能设计的公共方法
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task CreateOrUpdate(SaveCustomPagesAndAutomatic input)
        {
            if (input.CustomPage.Id.HasValue)
            {
                await Update(input.CustomPage, input.Trasferimnto, input.IsAuto);
            }
            else
            {
                await Create(input.CustomPage, input.Trasferimnto, input.IsAuto);
            }
        }

        /// <summary>
        ///     删除功能设计信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public async Task Delete(EntityDto<Guid> input)
        {
            await _customPageManager.DeleteAsync(input.Id);
        }

        /// <summary>
        ///     批量删除BaseCustomPage的方法
        /// </summary>
        public async Task BatchDelete(List<Guid> input)
        {
            await _customPageManager.BatchDelete(input);
        }

        /// <summary>
        /// 建表提示
        /// </summary>
        /// <returns></returns>
        public async Task<TrasferimentoEditDto> TableCreationPrompt(CreateOrUpdateCustomPageInput input)
        {
            var entity = await _automaticTablesManager.
                  CreateSqlStatement(input.CreateOrUpdateTable,
                  input.BaseCustomPageId.HasValue == true ? input.BaseCustomPageId.Value : Guid.Empty,
                  input.IsAuto);
            var dto = ObjectMapper.Map<TrasferimentoEditDto>(entity);
            return dto;
        }

        /// <summary>
        /// 自动建表
        /// </summary>
        /// <param name="tableDto"></param>
        /// <returns></returns>
        public Task AutoCreateTable(TrasferimentoEditDto tableDto)
        {
            var dto = ObjectMapper.Map<TrasferimentoDto>(tableDto);
            return _automaticTablesManager.FinalExecution(dto);
        }

        /// <summary>
        /// 新增功能设计
        /// </summary>
        protected virtual async Task<CustomPageEditDto> Create(CustomPageEditDto input,
            TrasferimentoEditDto tableDto, bool isAuto)
        {
            var entity = ObjectMapper.Map<BaseCustomPage>(input);

            entity.ColumnConfigJson = FromConsts.ColumnConfigJson;

            entity.TableConfigJson = FromConsts.TableConfigJson;

            using (var unitOfWork = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                entity = await _customPageManager.CreateAsync(entity);

                if (isAuto)
                {
                    tableDto.BaseCustomPageId = entity.Id;
                    await _automaticTablesManager.FinalExecution(ObjectMapper.Map<TrasferimentoDto>(tableDto));
                }

                unitOfWork.Complete();
            }

            var dto = ObjectMapper.Map<CustomPageEditDto>(entity);
            return dto;
        }

        /// <summary>
        /// 编辑功能设计
        /// </summary>
        protected virtual async Task Update(CustomPageEditDto input, TrasferimentoEditDto tableDto, bool isAuto)
        {
            var entity = await _customPageManager.QueryAsNoTracking.FirstOrDefaultAsync(x => x.Id == input.Id.Value);

            ObjectMapper.Map(input, entity);

            using (var unitOfWork = _unitOfWorkManager.Begin(TransactionScopeOption.Suppress))
            {
                await _customPageManager.UpdateAsync(entity);

                if (isAuto)
                {
                    tableDto.BaseCustomPageId = input.Id;
                    await _automaticTablesManager.FinalExecution(ObjectMapper.Map<TrasferimentoDto>(tableDto));
                }
                unitOfWork.Complete();
            }
        }

        /// <summary>
        /// 用户Ndo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>

        [HttpPost]
        public async Task<List<NdoStringDto>> GetUserNdo(LowCodeGetQueryFilterInput input)
        {
            var ret = await _userManager.Users
                            .WhereIf(!input.FilterText.IsNullOrEmpty(),
                            p => p.Name.Contains(input.FilterText))
                            .Select(x => new NdoStringDto { Id = x.Id, Name = x.UserName, CreationTime = x.CreationTime })
                            .OrderBy(input.Sorting)
                            .ToListAsync();
            return ret;
        }

        /// <summary>
        /// 角色Ndo
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<NdoStringDto>> GetRoleNdo(LowCodeGetQueryFilterInput input)
        {
            var ret = await _roleManager.Roles
                          .WhereIf(!input.FilterText.IsNullOrEmpty(),
                          p => p.Name.Contains(input.FilterText))
                          .Select(x => new NdoStringDto { Id = x.Id, Name = x.DisplayName, CreationTime = x.CreationTime })
                          .OrderBy(input.Sorting)
                          .ToListAsync();

            return ret;
        }

        /// <summary>
        /// 获取工艺参数卡配置
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public async Task<List<LowCodePpcDataPointDto>> GetPpcDataPointDto(string id)
        {
            var ret = new List<LowCodePpcDataPointDto>();

            ret.Add(new LowCodePpcDataPointDto()
            {
                Id = new Guid(),
                DataPointName = "外观",
                DataType = LowCodeDataPointDataType.Boolean,
                DisplayLimits = false,
                IsLimitOverrideAllowed = false,
                IsRequired = false,
                LowerLimit = null,
                UpperLimit = null,
                BooleanTrue = "透明",
                BooleanFalse = "半透明",
                Notes = null,
                Sequence = 1
            });
            ret.Add(new LowCodePpcDataPointDto()
            {
                Id = new Guid(),
                DataPointName = "固含量",
                DataType = LowCodeDataPointDataType.Decimal,
                DisplayLimits = true,
                IsLimitOverrideAllowed = false,
                IsRequired = false,
                LowerLimit = 8,
                UpperLimit = 10,
                BooleanTrue = null,
                BooleanFalse = null,
                Notes = null,
                Sequence = 2
            });
            ret.Add(new LowCodePpcDataPointDto()
            {
                Id = new Guid(),
                DataPointName = "粘度",
                DataType = LowCodeDataPointDataType.Decimal,
                DisplayLimits = true,
                IsLimitOverrideAllowed = false,
                IsRequired = false,
                LowerLimit = 5,
                UpperLimit = 20,
                BooleanTrue = null,
                BooleanFalse = null,
                Notes = null,
                Sequence = 3
            });
            ret.Add(new LowCodePpcDataPointDto()
            {
                Id = new Guid(),
                DataPointName = "密度",
                DataType = LowCodeDataPointDataType.Decimal,
                DisplayLimits = true,
                IsLimitOverrideAllowed = false,
                IsRequired = false,
                LowerLimit = 0.8,
                UpperLimit = 0.9,
                BooleanTrue = null,
                BooleanFalse = null,
                Notes = null,
                Sequence = 4
            });

            return ret;
        }

        //// custom codes

        //// custom codes end
    }
}
