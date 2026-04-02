# AppAuthAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/LowCodeAPI/AppAuthAppService.cs`

## 服务别名

- `AppAuthAppService`
- `IAppAuthAppService`

## 方法列表

- `public AppAuthAppService(IAppAuthManager authManager, IAppAuthMappingManager authMappingManager, IUnitOfWorkManager unitOfWorkManager)`

- `Task BatchDelete(List<Guid> input)`

- `Task CreateOrUpdate(CreateOrUpdateAppAuth input)`

- `Task Delete(EntityDto<Guid> input)`

- `Task<PagedResultDto<AppAuthListDto>> GetAppAuthList(GetAppAuthPagesInput input)`

- `Task<AppAuthListDto> GetById(EntityDto<Guid> input)`

- `Task CreateOrUpdateForAppAuthMapping(CreateOrUpdateAppAuthMapping input)`

- `Task<List<AppAuthMappingListDto>> GetAppAuthMappingList(GetAppAuthMappingPagesInput input)`

- `private string GenerateKey(int keySize)`

- `Task<int> CheckAppAuthYoyoApiGatewayToken(string AppKey, Guid AgentID)`
