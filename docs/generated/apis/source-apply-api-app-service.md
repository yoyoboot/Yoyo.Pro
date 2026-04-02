# ApplyAPIAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/LowCodeAPI/ApplyAPIAppService.cs`

## 服务别名

- `ApplyAPIAppService`
- `IApplyAPIAppService`

## 方法列表

- `public ApplyAPIAppService(IApplyAPIManager applyAPIManager , IApplyBodyDataManager bodyDataManager , IApplyQHDataManager qHDataManager , IUnitOfWorkManager unitOfWorkManager)`

- `Task BatchDelete(List<Guid> input)`

- `Task CreateOrUpdate(CreateOrUpdateApplyAPI input)`

- `Task Delete(EntityDto<Guid> input)`

- `Task<PagedResultDto<ApplyAPIListDto>> GetApplyAPIListByApplyID(GetApplyAPIPagesInput input)`

- `Task<ApplyAPIListDto> GetById(EntityDto<Guid> input)`

- `Task CreateOrUpdateAPISetting(CreateOrUpdateApplyAPI input)`

- `Task<List<ImprotSwaggerData>> ImprotSwagger(string url, Guid applyID)`

- `Task<bool> SureImprotSwagger(List<ImprotSwaggerData> swagger, Guid applyID)`

- `private void findChilderLeve(KeyValuePair<string, JsonProperty> ActualProperties, List<ApplyBodyDataListDto> listDtos, ImprotSwaggerData applyAPIList, ApplyBodyDataListDto parent, int leve, KeyValuePair<string, JsonProperty>? parentObject = null)`

- `private void RemoveCircularReferences(JToken jToken)`

- `private List<ApplyBodyDataListDto> findApplyBodyData(List<ApplyBodyDataListDto> applyQHDatas, List<ApplyBodyDataListDto> qHDatas)`
