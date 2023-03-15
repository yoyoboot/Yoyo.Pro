using System;
using System.Collections.Concurrent;
using System.Linq;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro.DbStore.FreeSqlStore.Partition
{
    public class DefaultIfSqlProviderDbStore : IFSqlProviderDbStore
    {
        public ConcurrentDictionary<string, IFSqlProvider> DataMap { get; private set; }


        public DefaultIfSqlProviderDbStore(IServiceProvider serviceProvider)
        {
            DataMap = new ConcurrentDictionary<string, IFSqlProvider>();

            var tmpDataMap = serviceProvider.GetServices<IFSqlProvider>()
                .ToDictionary(item => item.ProviderName);

            foreach (var item in tmpDataMap)
            {
                this.AddOrUpdate(item.Key, item.Value);
            }
        }


        public void AddOrUpdate(string name, IFSqlProvider val)
        {
            DataMap[name] = val;
        }

        public void Clear()
        {
            DataMap.Clear();
        }

        public IFSqlProvider GetByName(string name, string defaultName)
        {
            IFSqlProvider result;

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

            this.DataMap.TryRemove(name, out _);
        }
    }
}
