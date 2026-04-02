# LowCodePubAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/LowCodeAPI/LowCodePubAppService.cs`

## 服务别名

- `ILowCodePubAppService`
- `LowCodePubAppService`

## 方法列表

- `Task<PagedResultDto<DataApiList>> GetAPIList(GetAPIPagesInput input)`

- `Task<DataApiParameter> GetAPIParameter(Guid id)`

- `public LowCodePubAppService(IAgentAPIManager agentAPIManager, IApplyQHDataManager qhDataManager, IApplyBodyDataManager bodyDataManager, IApplyAPIManager applyAPIManager)`

- `internal List<DataApiField> FindChildren(List<ApplyBodyData> data, Guid? recordId)`
