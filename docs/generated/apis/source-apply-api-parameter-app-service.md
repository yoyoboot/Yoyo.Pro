# ApplyApiParameterAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/LowCodeAPI/ApplyApiParameterAppService.cs`

## 服务别名

- `ApplyApiParameterAppService`
- `IApplyApiParameterAppService`

## 方法列表

- `public ApplyApiParameterAppService(IApplyBodyDataManager applyBodyData, IApplyQHDataManager applyQHData , IUnitOfWorkManager unitOfWorkManager, IParameterMappingManager parameterMapping)`

- `Task BatchDeleteForApplyBodyData(List<Guid> input)`

- `Task BatchDeleteForApplyQHData(List<Guid> input)`

- `Task CreateOrUpdateForApplyBodyData(CreateOrUpdateApplyBodyData input)`

- `Task CreateOrUpdateForApplyQHData(CreateOrUpdateApplyQHData input)`

- `Task<PagedResultDto<ApplyBodyDataListDto>> GetApplyBodyDataList(GetPublicPagesInput input)`

- `Task<PagedResultDto<ApplyQHDataListDto>> GetApplyQHDataDataList(GetPublicPagesInput input)`

- `Task<List<ApplyQHDataListDto>> GetApiQueryData(EntityDto<Guid> input)`

- `Task<List<ApplyQHDataListDto>> GetApiHeaderData(EntityDto<Guid> input)`

- `Task<List<ApplyBodyDataListDto>> GetApiRequestBodyData(EntityDto<Guid> input, bool isPackage)`

- `Task<List<ApplyBodyDataListDto>> GetApiResponseBodyData(EntityDto<Guid> input, bool isPackage)`

- `internal List<ApplyBodyDataListDto> FindChildren(List<ApplyBodyDataListDto> data, Guid? recordId)`

- `Task CreateOrUpdateForParameterMapping(CreateOrUpdateParameterMapping input)`

- `Task BatchDeleteForParameterMapping(List<Guid> input)`

- `Task<PagedResultDto<ParameterMappingListDto>> GetParameterMappingList(GetPublicPagesInput input)`
