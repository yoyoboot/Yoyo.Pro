# BaseUserQueryGroupAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/UserQueryModule/UserQueryGroupDefinition/BaseUserQueryGroupAppService.cs`

## 服务别名

- `BaseUserQueryGroupAppService`
- `IBaseUserQueryGroupAppService`

## 方法列表

- `public BaseUserQueryGroupAppService(IUserQueryGroupManager userQueryGroupManager, IUserQueryGroupEntityManager userQueryGroupEntityManager, IUserQueryGroupSubManager userQueryGroupSubManager)`

- `Task<GetBaseUserQueryGroupForEditOutput> GetForEdit(NullableIdDto<Guid> input)`

- `Task<List<BaseUserQueryGroupListDto>> GetNdoCombox(LowCodeGetQueryFilterInput input)`

- `Task<PagedResultDto<BaseUserQueryGroupListDto>> GetNdoPaged(GetBaseUserQueryGroupInput input)`

- `Task<Guid> CreateOrUpdate(BaseUserQueryGroupCreateOrUpdateInput input)`

- `Task Delete(EntityDto<Guid> input)`

- `protected virtual async Task<Guid> Create(BaseUserQueryGroupEditDto input)`

- `protected virtual async Task<Guid> Update(BaseUserQueryGroupEditDto input)`
