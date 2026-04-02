# BaseUserQueryAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/UserQueryModule/UserQueryDefinition/BaseUserQueryAppService.cs`

## 服务别名

- `BaseUserQueryAppService`
- `IBaseUserQueryAppService`

## 方法列表

- `public BaseUserQueryAppService(IUserQueryManager userQueryManager, IUserQueryExecuter userQueryExecuter)`

- `Task<PagedResultDto<BaseUserQueryListDto>> GetNdoPaged(GetBaseUserQueryInput input)`

- `Task<List<BaseUserQueryListDto>> GetNdoCombox(LowCodeGetQueryFilterInput input)`

- `Task<List<LowCodeNdoDto>> GetNdo(LowCodeGetQueryFilterInput input)`

- `Task<List<ComboxDto<string>>> GetUserQueryParamterTypeCombox()`

- `Task<GetBaseUserQueryForEditOutput> GetForEdit(NullableIdDto<Guid> input)`

- `Task<Guid> CreateOrUpdate(BaseUserQueryCreateOrUpdateInput input)`

- `Task Delete(EntityDto<Guid> input)`

- `Task<BaseUserQueryTestOutput> Test(BaseUserQueryTestInput input)`

- `protected virtual async Task<Guid> Create(BaseUserQueryEditDto input)`

- `protected virtual async Task<Guid> Update(BaseUserQueryEditDto input)`
