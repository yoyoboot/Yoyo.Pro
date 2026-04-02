# ComFieldAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/ComField/ComFieldAppservice.cs`

## 服务别名

- `ComFieldAppService`
- `IComFieldAppService`

## 方法列表

- `public ComFieldAppService(IComFieldManager fieldManager)`

- `Task CreateOrUpdate(ComFieldForCreateOrUpdate input)`

- `await Update(input.ComField)`

- `await Create(input.ComField)`

- `protected virtual async Task<ComFieldEditDto> Create(ComFieldEditDto comField)`

- `private async Task Update(ComFieldEditDto comField)`

- `Task Delete(EntityDto<Guid> input)`

- `Task<ComFieldForEditOutput> GetForEdit(NullableIdDto<Guid> input)`

- `Task<PagedResultDto<ComFieldListDto>> GetPaged(ComFieldInput input)`

- `Task<List<ComFieldSelectorOutput>> GetComFieldSelector()`
