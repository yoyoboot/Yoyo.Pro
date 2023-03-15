using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro.DbStore.Repositories
{
    public class DefaultRelationalDatabaseProcessorDbStore : IRelationalDatabaseProcessorDbStore
    {
        public ConcurrentDictionary<string, IRelationalDatabaseProcessor> DataMap { get; private set; }

        public DefaultRelationalDatabaseProcessorDbStore(IServiceProvider serviceProvider)
        {
            DataMap = new ConcurrentDictionary<string, IRelationalDatabaseProcessor>();

            var tmpDataMap = serviceProvider.GetServices<IRelationalDatabaseProcessor>()
                .ToDictionary(item => item.DbStoreProviderName);

            foreach (var item in tmpDataMap)
            {
                this.AddOrUpdate(item.Key, item.Value);
            }
        }


        public void AddOrUpdate(string name, IRelationalDatabaseProcessor val)
        {
            DataMap[name] = val;
        }

        public void Clear()
        {
            DataMap.Clear();
        }

        public IRelationalDatabaseProcessor GetByName(string name, string defaultName)
        {
            IRelationalDatabaseProcessor result = null;

            if (name == null)
            {
                if (!DataMap.TryGetValue(defaultName, out result))
                {
                    throw new Exception("Unregistered default configuration");
                }
                return result;
            }
            else if (DataMap.TryGetValue(name, out result))
            {
                return result;
            }

            throw new ArgumentException($"The Provider with the name {name} was not found");
        }

        public void Remove(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                return;
            }

            this.DataMap.TryRemove(name, out IRelationalDatabaseProcessor result);
        }
    }
}
