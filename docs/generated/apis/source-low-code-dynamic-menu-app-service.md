# LowCodeDynamicMenuAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/DynamicMenus/LowCodeDynamicMenuAppService.cs`

## 服务别名

- `ILowCodeDynamicMenuAppService`
- `LowCodeDynamicMenuAppService`

## 方法列表

- `Task<PagedResultDto<LowCodeDynamicMenuListDto>> GetPaged(GetDynamicMenusInput input)`

- `Task<List<LowCodeDynamicMenuListDto>> GetList()`

- `Task<LowCodeDynamicMenuListDto> GetById(EntityDto<Guid> input)`

- `Task<GetDynamicMenuForEditOutput> GetForEdit(NullableIdDto<Guid> input)`

- `Task CreateOrUpdate(CreateOrUpdateDynamicMenuInput input)`

- `Task Delete(EntityDto<Guid> input)`

- `Task BatchDelete(List<Guid> input)`

- `Task Reset()`

- `public LowCodeDynamicMenuAppService(IDynamicMenuManager dynamicMenuManager)`

- `await Update(input.DynamicMenu)`

- `await Create(input.DynamicMenu)`

- `protected virtual async Task<LowCodeDynamicMenuEditDto> Create(LowCodeDynamicMenuEditDto input)`

- `protected virtual async Task Update(LowCodeDynamicMenuEditDto input)`

- `protected virtual async Task<string> GetNextChildCodeAsync(Guid? parentId)`

- `protected virtual async Task<BaseDynamicMenu> GetLastChildOrNullAsync(Guid? parentId)`

- `protected virtual async Task<string> GetCodeAsync(Guid id)`

- `private void CreateDynamicMenu(DynamicMenuCreateDto entity, string codeStr, Guid? parentId = null)`

- `private void CreateChildrenMenu(List<DynamicMenuCreateDto> entityList, string codeStr, Guid? parentId = null)`

- `private BaseDynamicMenu CreateBaseDynamicMenuData(BaseDynamicMenu dynamicMenu)`
