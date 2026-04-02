# AgentAPIAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/LowCodeAPI/AgentAPIAppService.cs`

## 服务别名

- `AgentAPIAppService`
- `IAgentAPIAppService`

## 方法列表

- `public AgentAPIAppService(IAgentAPIManager agentAPI, IAppAuthManager appAuth, IAppAuthMappingManager appAuthMapping, IUnitOfWorkManager unitOfWorkManager, IApplyBodyDataManager applyBodyDataManager, IApplyQHDataManager applyQHDataManager, IParameterMappingManager parameterMapping, IAPIRequestRecordManager apiLog, IFlowControlMappingManager flowControlMapping)`

- `Task BatchDelete(List<Guid> input)`

- `Task CreateOrUpdate(CreateOrUpdateAgentAPI input)`

- `Task Delete(EntityDto<Guid> input)`

- `Task<PagedResultDto<AgentAPIListDto>> GetAgentAPIList(GetAgentAPIPagesInput input)`

- `Task<AgentAPIListDto> GetById(EntityDto<Guid> input)`

- `Task<AgentAPIListDto> GetAgentAPIByPath(string path)`

- `Task<List<AgentAPIListAllDto>> GetAgentAPIListAll(GetAgentAPIPagesInput input)`

- `Task CreateOrUpdateAgentAPI(CreateOrUpdateAgentAPI input)`

- `Task SetAPIEnableStatus(EntityDto<Guid> input, bool enableStatus)`

- `Task<PagedResultDto<APIRequestRecordListDto>> GetAgentAPIRequestRecord(GetAPIRequestRecordPagesInput input)`

- `Task<PagedResultDto<APIRequestRecordListDto>> GetAgentAPIRequestRecordList(GetAPIRequestRecordPagesInput input)`

- `Task<Dashboard_TJDto> GetDashboardTJ(Dashboard_TJInput input)`
