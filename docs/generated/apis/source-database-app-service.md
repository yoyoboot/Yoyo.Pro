# DatabaseAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/Databases/DatabaseAppService.cs`

## 服务别名

- `DatabaseAppService`
- `IDatabaseAppService`

## 方法列表

- `public DatabaseAppService(IDatabaseManager databaseManager)`

- `Task<List<DatabaseTableListOutput>> GetTableList(GetDatabaseTableListInput input)`

- `Task<List<DbTableFieldModel>> GetTableFieldList(GetTableFieldListInput input)`

- `Task<List<TableFieldsSelectorOutput>> GetTableFieldSelect(GetTableFieldListInput input)`

- `Task<PagedResultDto<object>> GetTableData(GetTableDataInput input)`

- `Task DeleteTable(DeleteTableInput input)`

- `Task CreateTable(CreateOrUpdateTableInput input)`

- `Task UpdateTable(CreateOrUpdateTableInput input)`
