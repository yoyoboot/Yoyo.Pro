namespace Yoyo.Pro.DbStore.Partition
{
    public interface IPartitionTableNameFactory
    {
        /// <summary>
        /// 获取表名
        /// </summary>
        /// <param name="partitionId">分表键值</param>
        /// <returns>表名</returns>
        string GetTableName(string basicTableName, object partitionId);
    }
}
