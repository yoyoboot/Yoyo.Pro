# CustomPageAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/CustomPages/CustomPageAppService.cs`

## 服务别名

- `CustomPageAppService`
- `ICustomPageAppService`

## 方法列表

- `public CustomPageAppService(ICustomPageManager customPageManager, LowCodeUserManager userManager, LowCodeRoleManager roleManager, IAutomaticTablesManager automaticTablesManager, IUnitOfWorkManager unitOfWorkManager, ILowCodeModelRelationManager modelRelationManager)`

- `Task<PagedResultDto<CustomPageListDto>> GetPaged(GetCustomPagesInput input)`

- `Task<List<CustomPageListDto>> GetList(string filterText)`

- `Task<CustomPageListDto> GetById(EntityDto<Guid> input)`

- `Task<GetCustomPageForEditOutput> GetForEdit(NullableIdDto<Guid> input)`

- `Task CreateOrUpdate(SaveCustomPagesAndAutomatic input)`

- `await Update(input.CustomPage, input.Trasferimnto, input.IsAuto)`

- `await Create(input.CustomPage, input.Trasferimnto, input.IsAuto)`

- `Task Delete(EntityDto<Guid> input)`

- `Task BatchDelete(List<Guid> input)`

- `Task<TrasferimentoEditDto> TableCreationPrompt(CreateOrUpdateCustomPageInput input)`

- `Task AutoCreateTable(TrasferimentoEditDto tableDto)`

- `protected virtual async Task<CustomPageEditDto> Create(CustomPageEditDto input, TrasferimentoEditDto tableDto, bool isAuto)`

- `protected virtual async Task Update(CustomPageEditDto input, TrasferimentoEditDto tableDto, bool isAuto)`

- `Task<List<NdoStringDto>> GetUserNdo(LowCodeGetQueryFilterInput input)`

- `Task<List<NdoStringDto>> GetRoleNdo(LowCodeGetQueryFilterInput input)`

- `Task<List<LowCodePpcDataPointDto>> GetPpcDataPointDto(string id)`

- `Task<List<CustomPageListDto>> GetList(string FilterText)`
