# 标识符双引号与大小写自动处理方案（Oracle/PostgreSQL）

## 目标

在 SQL 生成中继续保留双引号 `""` 包裹标识符，同时自动处理不同数据库默认大小写规则：
- Oracle：未加引号对象默认折叠为大写。
- PostgreSQL：未加引号对象默认折叠为小写。

并提供全局开关，在某些场景下可以关闭“自动大小写处理”，仅保留双引号引用行为。

目标是避免因为加了双引号导致大小写不匹配（如 `"Order"` 找不到 `ORDER` 或 `order`），并兼容用户显式定义的大小写对象。

## 现状问题

当前实现主要是字符串层面的方括号替换：
- PostgreSQL：`[`/`]` -> `"`/`"`
- Oracle：`[`/`]` -> 空

这种方式没有“名字解析”步骤，无法区分：
1. 默认命名对象（应按数据库规则折叠）
2. 用户显式大小写对象（应严格保留）

## 总体设计

增加一个统一的“标识符解析与引用”层，所有 QueryBuilder 在拼接表名/列名前先走该层。

### 1. 核心组件

新增组件建议：`IdentifierFormatter`（可放在 SQLAdapters 下）

职责：
- 解析 schema/table/column 原始名字
- 自动推断最终大小写
- 统一输出双引号包裹的标识符

建议方法：
- `NormalizeAndQuoteMultipart(DbContext context, DbServerType dbType, string rawName, IdentifierKind kind)`
- `NormalizeAndQuoteSingle(DbContext context, DbServerType dbType, string rawName, IdentifierKind kind)`
- `ResolvePhysicalName(...)`（通过系统目录查询真实大小写）

### 2. 决策优先级（从高到低）

全局开关：
- `EnableIdentifierAutoCaseResolve = false` 时：跳过自动解析和大小写折叠，直接按输入名（去外壳后）加双引号。
- `EnableIdentifierAutoCaseResolve = true` 时：按下面优先级执行。

1) 显式引用优先
- 输入已是双引号形式（如 `"MyTable"`）时，视为用户显式指定，保留内部大小写，不再折叠。

2) 元数据解析优先
- 通过系统目录按不区分大小写匹配真实对象名，并返回真实大小写：
  - Oracle: `ALL_TABLES` / `ALL_TAB_COLUMNS`
  - PostgreSQL: `information_schema.tables` / `information_schema.columns`

3) 数据库默认折叠兜底
- Oracle: `ToUpperInvariant()`
- PostgreSQL: `ToLowerInvariant()`

最终统一输出双引号：
- 表名：`"SCHEMA"."TABLE"`
- 列名：`"COLUMN"`

### 3. 为什么必须“解析后再加引号”

数据库对未加引号标识符会先折叠再匹配；一旦加引号就会“区分大小写精确匹配”。
因此正确流程应为：

1. 先推导物理名真实大小写
2. 再加双引号

而不是先把任意输入直接包上双引号。

## 可配置策略（建议）

在 BulkConfig 增加配置（向后兼容）：

```csharp
public bool EnableIdentifierAutoCaseResolve { get; set; } = true;

public enum IdentifierCaseStrategy
{
    Auto = 0,   // 默认：先元数据解析，失败后按数据库默认折叠
    Preserve,   // 保持输入大小写，仅加双引号
    Upper,      // 强制大写后加双引号
    Lower       // 强制小写后加双引号
}

public IdentifierCaseStrategy IdentifierCaseStrategy { get; set; } = IdentifierCaseStrategy.Auto;
```

行为约束建议：
- 当 `EnableIdentifierAutoCaseResolve = false`：忽略 `IdentifierCaseStrategy` 的自动分支（包括目录查询和大小写折叠）。
- 当 `EnableIdentifierAutoCaseResolve = true`：按 `IdentifierCaseStrategy` 执行。

适用场景：
- Auto：推荐默认。
- Preserve：已有大量显式大小写对象（例如历史遗留 `"CamelCase"`）。
- Upper/Lower：库内命名规范高度统一时可用。
- 全局关闭自动处理：多租户跨库、历史库命名混乱、或业务方已明确传入正确物理名时可用。

## 缓存与性能

为避免每次拼 SQL 都查询系统目录，建议做连接级缓存：
- Key: `(dbType, schema, objectName, kind)`
- Value: `resolvedPhysicalName`
- 生命周期：单次 Bulk 操作（或单个 DbContext）

这样首次解析后可复用，几乎不增加批量场景开销。

## 建议改造点

1. 在各数据库 QueryBuilder 中，替换简单的字符串替换逻辑。
2. 表名来源（如 FullTableName/FullTempTableName）进入 SQL 前调用 IdentifierFormatter。
3. 列名来源（PropertyColumnNamesDict 等）在拼接前逐个调用 IdentifierFormatter。
4. 保留现有 SQL 模板结构，最小化侵入。

## 与现有代码的对应关系

当前重点位置：
- PostgreSQL 查询构建：[SQLAdapters/PostgreSql/SqlQueryBuilderPostgreSql.cs](SQLAdapters/PostgreSql/SqlQueryBuilderPostgreSql.cs)
- Oracle 查询构建：[SQLAdapters/Oracle/SqlQueryBuilderOracle.cs](SQLAdapters/Oracle/SqlQueryBuilderOracle.cs)
- 表/列元数据来源：[TableInfo.cs](TableInfo.cs)

## 分阶段落地建议

Phase 1（低风险）
- 新增 IdentifierFormatter 与缓存
- 先接入 PostgreSQL、Oracle 的建表/合并/删除主路径

Phase 2（完善）
- 接入批量 Update/Delete 重写逻辑
- 对 CustomSourceTableName/CustomDestinationTableName 增加显式引用识别

Phase 3（验证）
- Oracle 用例：默认未引用建表（全大写）、显式引用建表（混合大小写）
- PostgreSQL 用例：默认未引用建表（全小写）、显式引用建表（混合大小写）
- 回归：SQL Server/MySQL/SQLite 不受影响

## 关键测试点

1. 默认命名对象
- Oracle: 输入 `Order` 应解析为 `"ORDER"`
- PostgreSQL: 输入 `Order` 应解析为 `"order"`

2. 显式大小写对象
- 输入 `"Order"` 必须保持 `"Order"`

3. schema + table
- 输入 `Sales.Order` 在 Oracle 解析为 `"SALES"."ORDER"`
- 输入 `Sales.Order` 在 PostgreSQL 解析为 `"sales"."order"`

4. 列名映射
- Property->Column 的自定义映射仍需命中真实列

## 伪代码示例

```csharp
string ResolveIdentifier(DbContext ctx, DbServerType dbType, string raw, IdentifierKind kind, IdentifierCaseStrategy strategy)
{
    var parsed = StripWrapper(raw); // 去掉 []/"" 外壳，保留原始内容

    if (!bulkConfig.EnableIdentifierAutoCaseResolve)
        return Quote(parsed);

    if (IsExplicitQuoted(raw) || strategy == IdentifierCaseStrategy.Preserve)
        return Quote(parsed);

    var resolved = strategy switch
    {
        IdentifierCaseStrategy.Upper => parsed.ToUpperInvariant(),
        IdentifierCaseStrategy.Lower => parsed.ToLowerInvariant(),
        _ => ResolveFromCatalogOrDefault(ctx, dbType, parsed, kind)
    };

    return Quote(resolved);
}
```

## 总结

该方案的关键不是“是否加双引号”，而是“先解析物理名，再加双引号”。
通过统一的标识符解析层，可以在保留 `""` 的前提下同时兼容：
- Oracle 默认大写
- PostgreSQL 默认小写
- 用户显式大小写对象

同时通过全局开关可以随时降级为“仅引用不改写”的安全模式，降低对历史命名差异库的接入风险。
