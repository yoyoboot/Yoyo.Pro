// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Auditing;
using MongoDB.Driver;
using MongoDB.Driver.Linq;

namespace Yoyo.Pro.Auditing
{
    public static class MongoAuditLogRepositoryExtensions
    {
        /// <summary>
        /// 审计日志MongoDb查询配置
        /// </summary>
        public static AggregateOptions QueryAggregateOptions { get; set; } = new AggregateOptions()
        {
            AllowDiskUse = true
        };

        public static IQueryable<TAuditLog> GetAll<TAuditLog>(this IAuditLogRepository<TAuditLog> repository, DateTime dateTime)
            where TAuditLog : AuditLogs2, new()
        {
            if (repository is MongoAuditLogRepository<TAuditLog> mongoRepo)
            {
                return mongoRepo.GetTable(dateTime)
                    .AsQueryable(QueryAggregateOptions);
            }


            return repository.GetAll()
                        ;
        }
    }
}
