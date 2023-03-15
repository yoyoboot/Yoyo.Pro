using System;
using System.Reflection;

namespace Yoyo.Pro.DbStore.Repositories
{
    public class DefaultRelationalDatabaseProcessor : IRelationalDatabaseProcessor
    {
        protected readonly string _dbStorageProviderName;
        protected readonly Type _dbStorageProviderType;
        protected readonly DatabaseType _databaseType;
        public string DbStoreProviderName => this._dbStorageProviderName;

        public Type DbStoreProviderType => this._dbStorageProviderType;

        public DefaultRelationalDatabaseProcessor(
            Type dbStorageProviderType,
            DatabaseType databaseType,
            string providerName)
        {
            _dbStorageProviderType = dbStorageProviderType;
            _databaseType = databaseType;
            _dbStorageProviderName = providerName;
        }

        public virtual string HandleConnectionString(string connectionString)
        {
            switch (_databaseType)
            {
                case DatabaseType.Oracle:
                    break;
            }

            return connectionString;
        }

        public virtual string HandleEntityName(Type entityType)
        {
            return HandleString(entityType.Name);
        }


        public string HandleEntityProperty(Type entityType, PropertyInfo propertyInfo)
        {
            return HandleString(propertyInfo.Name);
        }



        public string HandleString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                throw new ArgumentNullException(nameof(input));
            }

            switch (_databaseType)
            {
                case DatabaseType.PostgreSql:
                    return input.ToLower();
                case DatabaseType.Oracle:
                    return input.ToUpper();
            }

            return input;
        }
    }
}
