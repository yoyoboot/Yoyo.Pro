# RenderAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/Renders/RenderAppService.cs`

## 服务别名

- `IRenderAppService`
- `RenderAppService`

## 方法列表

- `Task<PagedResultDto<object>> GetPaged(GetRenderPageInput pageInput)`

- `Task<string> GetById(GetRenderInput input)`

- `Task<string> CreateOrUpdate(RenderCreateOrUpdateInput input)`

- `Task Delete(GetRenderInput input)`

- `Task BatchDelete(BatchDeleteDto input)`

- `Task<LowCodeSysFileListDto> UploadFile()`

- `Task<LowCodeSysFileListDto> GetFileById(Guid id)`

- `public RenderAppService(IRenderManager renderManager, IHttpContextAccessor httpContextAccessor, IBasicFileManager sysFileManager, ICustomPageManager customPageManager, ILowCodeModelRelationManager lowCodeModelRelationManager)`

- `private async Task<List<LowCodeModelRelation>> GetrelationList(Guid id)`

- `throw new UserFriendlyException("错误", $"表BaseCustomPage中{input.CustomPageId}不存在")`
