# LowCodeDataDictionaryAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/DataDictionarys/LowCodeDataDictionaryAppService.cs`

## 服务别名

- `ILowCodeDataDictionaryAppService`
- `LowCodeDataDictionaryAppService`

## 方法列表

- `Task<PagedResultDto<LowCodeDataDictionaryListDto>> GetPaged(DataDictionarysInput input)`

- `Task<List<LowCodeDataDictionaryListDto>> GetList(string FilterText)`

- `Task<LowCodeDataDictionaryListDto> GetById(EntityDto<Guid> input)`

- `Task<GetLowCodeDataDictionaryForEditOutput> GetForEdit(NullableIdDto<Guid> input)`

- `Task CreateOrUpdate(CreateOrUpdateDictonary input)`

- `Task Delete(EntityDto<Guid> input)`

- `Task BatchDelete(List<Guid> input)`

- `Task<List<LowCodeDataDictionaryListDto>> SelectDataDictionary()`

- `Task<List<SelectDictionaryValue>> SelectDataDictionaryvalue(EntityDto<Guid> input)`

- `public LowCodeDataDictionaryAppService(IDataDictionarysManager dataDictionarys, IDataDictionarysValueManager dataDictionarysValue)`

- `await Update(input.LowCodeDataDictionaryPage)`

- `await Create(input.LowCodeDataDictionaryPage)`

- `Task<List<LowCodeDataDictionaryListDto>> GetList(string filterText)`

- `protected virtual async Task<LowCodeDataDictionaryEditDto> Create(LowCodeDataDictionaryEditDto input)`

- `protected async Task<List<BaseDictionaryValue>> ProcessEntrys(BaseDictionaryType entity, LowCodeDataDictionaryEditDto input)`

- `protected virtual async Task Update(LowCodeDataDictionaryEditDto input)`

- `object Json()`
