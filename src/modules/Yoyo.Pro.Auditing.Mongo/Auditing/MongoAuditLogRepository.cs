using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Abp;
using Abp.Auditing;
using Yoyo.Pro.Configuration;
using Yoyo.Pro.Mongo;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Yoyo.Pro.Auditing
{
    public class MongoAuditLogRepository<TAuditLog> : IAuditLogRepository<TAuditLog>
        where TAuditLog : AuditLogs2, new()
    {
        /// <summary>
        /// 连接提供者
        /// </summary>
        public virtual IMongoConnectionProvider ConnectionProvider { get; }

        /// <summary>
        /// 索引检查器
        /// </summary>
        public virtual IMongoAuditLogRepositoryIndexCheck<TAuditLog> IndexChecker { get; }


        public MongoAuditLogRepository(IMongoConnectionProvider connectionProvider, IMongoAuditLogRepositoryIndexCheck<TAuditLog> indexChecker)
        {
            this.ConnectionProvider = connectionProvider;
            this.IndexChecker = indexChecker;
        }

        public virtual IQueryable<TAuditLog> GetAll()
        {
            return this.GetTable().AsQueryable(
                MongoAuditLogRepositoryExtensions.QueryAggregateOptions
                );
        }

        public virtual void Insert(TAuditLog auditLog)
        {
            auditLog.Id = ObjectId.GenerateNewId().ToString();
            this.GetTable(auditLog.ExecutionTime).InsertOne(auditLog);
        }

        public virtual async Task InsertAsync(TAuditLog auditLog)
        {
            auditLog.Id = ObjectId.GenerateNewId().ToString();
            await this.GetTable(auditLog.ExecutionTime).InsertOneAsync(auditLog);
        }

        public async Task DeleteAsync(List<TAuditLog> auditLog, DateTime? startTime = null)
        {
            await this.GetTable(startTime).DeleteManyAsync(x => auditLog.Contains(x));
        }

        public void Delete(List<TAuditLog> auditLog, DateTime? startTime = null)
        {
            this.GetTable(startTime).DeleteManyAsync(x => auditLog.Contains(x));
        }


        /// <summary>
        /// 获取表
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public virtual IMongoCollection<TAuditLog> GetTable(DateTime? dateTime = null)
        {
            var tableName = ConnectionProvider.AuditLogType.Name;
            if (dateTime != null)
            {
                tableName = $"{ConnectionProvider.AuditLogType.Name}_{dateTime.Value.ToString("yyyyMM")}";
            }

            var table = ConnectionProvider.GetDatabase()
                        .GetCollection<TAuditLog>(tableName)
                        ;

            this.IndexChecker.Check(tableName, table);

            return table;
        }
    }

}
