using System;

namespace Yoyo.Pro.DbStore.FreeSqlStore.Partition
{
    public interface IFSqlProvider : IDisposable
    {
        /// <summary>
        /// 
        /// </summary>
        string ProviderName { get; }

        /// <summary>
        /// FreeSql实例
        /// </summary>
        IFreeSql FSql { get; }
    }
}
