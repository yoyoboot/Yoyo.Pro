using System;

namespace Yoyo.Pro.DbStore.Partition
{
    public class PartitionTableNameFactory : IPartitionTableNameFactory
    {
        protected readonly IServiceProvider _serviceProvider;

        /// <summary>
        /// 分表的数量
        /// </summary>
        protected readonly int _hashTableCount;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <param name="hashTableCount"></param>
        public PartitionTableNameFactory(IServiceProvider serviceProvider, int hashTableCount = 1000)
        {
            _serviceProvider = serviceProvider;
            _hashTableCount = hashTableCount;
        }

        public  virtual string GetTableName(string basicTableName, object partitionId)
        {
            if (partitionId == null) return basicTableName;

            if (partitionId is DateTime dt) return $"{basicTableName}{dt:yyyyMM}";

            if (partitionId is string s) return $"{basicTableName}_S_{s}";

            return $"{basicTableName}_P_{(long)partitionId % _hashTableCount}";
        }
    }
}
