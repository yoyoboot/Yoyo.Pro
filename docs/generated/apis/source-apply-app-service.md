# ApplyAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/LowCodeAPI/ApplyAppService.cs`

## 服务别名

- `ApplyAppService`
- `IApplyAppService`

## 方法列表

- `public ApplyAppService(IApplyManager applyTypeManager, IApplyHealthTestingManager applyHealthTestingManager, IApplyAuthenticationManager authenticationManager, IUnitOfWorkManager unitOfWorkManager, ICacheManager cacheManager)`

- `Task BatchDelete(List<Guid> input)`

- `Task CreateOrUpdate(CreateOrUpdateApply input)`

- `Task Delete(EntityDto<Guid> input)`

- `Task<PagedResultDto<ApplyListDto>> GetApplyList(GetApplyPagesInput input)`

- `Task<ApplyListDto> GetById(EntityDto<Guid> input)`

- `Task<ApplyListDto> GetByCode(string code)`

- `Task UpdateApplySetting(UpdateApplySettingInput input)`

- `Task<List<TestingApplyHealthDto>> GetApplyHearth()`
