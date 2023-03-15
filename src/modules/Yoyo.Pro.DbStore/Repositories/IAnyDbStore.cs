using System.Collections.Concurrent;

namespace Yoyo.Pro.DbStore.Repositories
{
    public interface IAnyDbStore<T>
        where T : class
    {
        ConcurrentDictionary<string, T> DataMap { get; }

        T GetByName(string name, string defaultName);


        void AddOrUpdate(string name, T val);


        void Remove(string name);


        void Clear();
    }
}
