# DbLinkAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/DbLinks/DbLinkAppService.cs`

## 服务别名

- `DbLinkAppService`
- `IDbLinkAppService`

## 方法列表

- `public DbLinkAppService(IDbLinkManager dbLinkManager)`

- `Task<PagedResultDto<DbLinkListDto>> GetPaged(GetDbLinksInput input)`

- `Task<DbLinkListDto> GetById(EntityDto<Guid> input)`

- `Task<GetDbLinkForEditOutput> GetForEdit(NullableIdDto<Guid> input)`

- `Task CreateOrUpdate(CreateOrUpdateDbLinkInput input)`

- `await Update(input.DbLink)`

- `await Create(input.DbLink)`

- `Task Delete(EntityDto<Guid> input)`

- `Task BatchDelete(List<Guid> input)`

- `Task TestDbConnection(DbLinkListDto input)`

- `Task<List<DbLinkSelectorOutput>> GetDbSelector()`

- `protected virtual async Task<DbLinkEditDto> Create(DbLinkEditDto input)`

- `protected virtual async Task Update(DbLinkEditDto input)`
