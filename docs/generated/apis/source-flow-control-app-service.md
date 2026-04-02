# FlowControlAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/LowCodeAPI/FlowControlAppService.cs`

## 服务别名

- `FlowControlAppService`
- `IFlowControlAppService`

## 方法列表

- `public FlowControlAppService(IFlowControlManager flowManager, IFlowControlMappingManager flowMappingManager, IUnitOfWorkManager unitOfWorkManager)`

- `Task BatchDelete(List<Guid> input)`

- `Task CreateOrUpdate(CreateOrUpdateFlowControl input)`

- `Task Delete(EntityDto<Guid> input)`

- `Task<PagedResultDto<FlowControlListDto>> GetFlowControlList(GetFlowControlPagesInput input)`

- `Task<FlowControlListDto> GetById(EntityDto<Guid> input)`

- `Task CreateOrUpdateForFlowControlMapping(CreateOrUpdateFlowControlMapping input)`

- `Task<List<FlowControlMappingEditDto>> GetFlowControlMappingList(GetFlowControlMappingPagesInput input)`

- `Task<FlowControl> GetFlowControlByAgentID(Guid AgentID)`
