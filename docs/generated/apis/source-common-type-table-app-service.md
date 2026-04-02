# CommonTypeTableAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/LowCodeAPI/CommonTypeTableAppService.cs`

## 服务别名

- `CommonTypeTableAppService`
- `ICommonTypeTableAppService`

## 方法列表

- `public CommonTypeTableAppService(ICommonTypeTableManager commonTypeTable)`

- `Task BatchDelete(List<Guid> input)`

- `Task CreateOrUpdate(CreateOrUpdateCommonTypeTable input)`

- `Task Delete(EntityDto<Guid> input)`

- `Task<PagedResultDto<CommonTypeTableListDto>> GetApplyAPIList(GetCommonTypeTablePagesInput input)`

- `Task<CommonTypeTableListDto> GetById(EntityDto<Guid> input)`
