# LowCodeViewModelsAppService

- 生成时间：`2026-03-28T02:51:16Z`
- 来源：`C# source scan`
- 源文件：`C:/Code/yoyoboot/Yoyo.Pro/src/modules-business/lowcode/Yoyo.LowCode.Shared.Application/LowCodeViewModels/LowCodeViewModelsAppService.cs`

## 服务别名

- `ILowCodeViewModelsAppService`
- `LowCodeViewModelsAppService`

## 方法列表

- `Task<List<TableListOutput>> GetTableAsTable(GetDatabaseTableListInput input)`

- `Task<List<TableListSelectOutput>> GetTableAsSelect(GetDatabaseTableListInput input)`

- `Task<List<TableListSelectOutput>> GetTableByAssociation(string mainTableName)`

- `Task<List<LowCodeFieldListDto>> GetTableFieldAsTable(GetTableFieldListInput input)`

- `Task<string> GetTablePrimaryKey(GetTableFieldListInput input)`

- `Task<List<LowCodeTableFieldsSelectorOutput>> GetTableFieldAsSelect(GetTableFieldListInput input)`

- `Task<PagedResultDto<object>> GetTableData(GetTableDataInput input)`

- `Task DeleteTableData(DeleteTableDataInput input)`

- `Task CreateTable(LowCodeCreateOrUpdateTableInput input)`

- `Task UpdateTable(LowCodeCreateOrUpdateTableInput input)`

- `Task<bool> TableIsExists(List<TableInfoList> list)`

- `public LowCodeViewModelsAppService(IViewModelManager viewPageManager, ILowCodeModelRelationManager LowCodeModelRelationManager, IDatabaseManager databaseManager, ILowCodeFieldManager fieldCollectionManager, IUnitOfWorkManager unitOfWorkManager, IWebHostEnvironment env = null)`

- `protected async Task<List<DbTableModel>> GetTableList(GetDatabaseTableListInput input)`

- `protected async Task<List<LowCodeField>> GetTableFieldList(GetTableFieldListInput input)`

- `await CreateOrUpdateLowCodeModel(input.NewTableInfo)`

- `await CreateField(input.TableFieldList)`

- `await ProcessEntrysField(input.NewTableInfo, input.TableFieldList)`

- `await ProcessingRedundantTableInformation(input.NewTableInfo.OldTableName)`

- `private async Task ProcessingRedundantTableInformation(string tableName)`

- `protected virtual async Task CreateField(List<LowCodeFieldEditDto> fieldList)`

- `protected async Task ProcessEntrysField(NewTableInfo tableInfo, List<LowCodeFieldEditDto> lowCodeFieldEditsList)`

- `protected async Task<LowCodeModelRelation> CreateOrUpdateModelRelation(LowCodeModelRelation entity, Guid? id)`

- `protected async Task CreateOrUpdateLowCodeModel(NewTableInfo tableInfo)`

- `bool Equals(LowCodeField x, LowCodeField y)`

- `int GetHashCode([DisallowNull] LowCodeField obj)`
