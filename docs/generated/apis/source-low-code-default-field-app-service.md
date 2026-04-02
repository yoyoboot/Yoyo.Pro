# LowCodeDefaultFieldAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/LowCodeDefaultFields/LowCodeDefaultFieldAppService.cs`

## 服务别名

- `ILowCodeDefaultFieldAppService`
- `LowCodeDefaultFieldAppService`

## 方法列表

- `Task<PagedResultDto<LowCodeDefaultFieldListDto>> GetPaged(LowCodeDefaultFieldInput input)`

- `Task CreateOrUpdate(LowCodeDefaultFieldCreateOrUpdate defaultField)`

- `Task Delete(EntityDto<Guid> input)`

- `Task BatchDelete(List<Guid> input)`

- `Task<LowCodeDefaultFieldForEditOutput> GetForEdit(NullableIdDto<Guid> input)`

- `public LowCodeDefaultFieldAppService(IDefaultFieldManager defaultFieldManager)`

- `await Update(defaultField.LowCodeDefaultField)`

- `await Create(defaultField.LowCodeDefaultField)`

- `protected virtual async Task<LowCodeDefaultFieldEditDto> Create(LowCodeDefaultFieldEditDto fieldEditDto)`

- `await FieldIsExists(fieldEditDto.FieldName)`

- `protected async Task Update(LowCodeDefaultFieldEditDto fieldEditDto)`

- `private async Task FieldIsExists(string fieldName)`
