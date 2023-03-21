// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yoyo.Pro.Configuration;
using MongoDB.Driver;


namespace Yoyo.Pro.Mongo
{
    public interface IMongoConnectionProvider
    {
        Type AuditLogType { get; }

        MongoClient GetClient();

        IMongoDatabase GetDatabase();

    }

    public class MongoConnectionProvider : IMongoConnectionProvider, Abp.Dependency.ISingletonDependency
    {
        readonly IYoyoProAuditingConfiguration _abpAuditingConfiguration;

        readonly Lazy<MongoClient> _lazyLoadMongoClient;
        readonly Lazy<string> _lazyLoadDataBaseName;

        public MongoConnectionProvider(IYoyoProAuditingConfiguration abpAuditingConfiguration)
        {
            _abpAuditingConfiguration = abpAuditingConfiguration;

            _lazyLoadMongoClient = new Lazy<MongoClient>(() =>
              {
                  return new MongoClient(_abpAuditingConfiguration.GetMongoConnectionString());
              });
            _lazyLoadDataBaseName = new Lazy<string>(() =>
            {
                return _abpAuditingConfiguration.GetMongoDatabaseName();
            });
        }

        public Type AuditLogType => _abpAuditingConfiguration.AuditLogType;

        public MongoClient GetClient()
        {
            return _lazyLoadMongoClient.Value;
        }


        public IMongoDatabase GetDatabase()
        {
            var client = this.GetClient();
            return client.GetDatabase(_lazyLoadDataBaseName.Value);
        }

    }

}
