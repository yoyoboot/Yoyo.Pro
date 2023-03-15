// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using MongoDB.Bson;
using MongoDB.Driver;

namespace Yoyo.Pro.Auditing
{
    /// <summary>
    /// 索引检查器
    /// </summary>
    /// <typeparam name="TAuditLog"></typeparam>
    public interface IMongoAuditLogRepositoryIndexCheck<TAuditLog>
         where TAuditLog : AuditLogs2, new()
    {
        /// <summary>
        /// 检查表索引
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="table"></param>
        void Check(string tableName, IMongoCollection<TAuditLog> table);
    }

    public class MongoAuditLogRepositoryIndexCheck<TAuditLog> : IMongoAuditLogRepositoryIndexCheck<TAuditLog>
         where TAuditLog : AuditLogs2, new()
    {
        /// <summary>
        /// 索引检查记录
        /// </summary>
        public virtual Dictionary<string, bool> IndexCheckedDict { get; }

        /// <summary>
        /// 创建索引模型
        /// </summary>
        public virtual List<CreateIndexModel<TAuditLog>> CreateIndexModels { get; }


        public MongoAuditLogRepositoryIndexCheck()
        {
            IndexCheckedDict = new Dictionary<string, bool>();
            CreateIndexModels = this.GenIndexs();
        }


        /// <summary>
        /// 预生成所有索引
        /// </summary>
        protected virtual List<CreateIndexModel<TAuditLog>> GenIndexs()
        {
            // 索引选项
            var indexOptions = new CreateIndexOptions()
            {
                Background = true
            };

            // 索引定义

            var indexList = new List<CreateIndexModel<TAuditLog>>();

            // 执行时间
            var executionTimeKey = new BsonDocument(
                nameof(AuditLogs2.ExecutionTime),
                1);
            var executionTimeIndex = new CreateIndexModel<TAuditLog>(
                  executionTimeKey,
                  indexOptions
                  );
            indexList.Add(executionTimeIndex);

            // 服务名称
            var serviceNameKey = new BsonDocument(
                nameof(AuditLogs2.ServiceName),
                1);
            var serviceNameIndex = new CreateIndexModel<TAuditLog>(
                  serviceNameKey,
                  indexOptions
                  );
            indexList.Add(serviceNameIndex);

            // 方法名称
            var methodNameKey = new BsonDocument(
                nameof(AuditLogs2.MethodName),
                1);
            var methodNameIndex = new CreateIndexModel<TAuditLog>(
                  methodNameKey,
                  indexOptions
                  );
            indexList.Add(methodNameIndex);

            // 持续时间
            var executionDurationKey = new BsonDocument(
                nameof(AuditLogs2.ExecutionDuration),
                1);
            var executionDurationIndex = new CreateIndexModel<TAuditLog>(
                  executionDurationKey,
                  indexOptions
                  );
            indexList.Add(executionDurationIndex);



            return indexList;
        }



        /// <summary>
        /// 检查索引
        /// </summary>
        /// <param name="tableName"></param>
        /// <param name="table"></param>
        public virtual void Check(string tableName, IMongoCollection<TAuditLog> table)
        {
            // 已经检查过索引，跳过
            if (IndexCheckedDict.TryGetValue(tableName, out var @checked) && @checked)
            {
                return;
            }

            // 添加到已检查容器
            IndexCheckedDict[tableName] = true;

            // 没有索引，跳过
            if (CreateIndexModels == null || CreateIndexModels.Count == 0)
            {
                return;
            }

            // 创建索引
            var res = table.Indexes.CreateMany(this.CreateIndexModels);
        }

    }
}
