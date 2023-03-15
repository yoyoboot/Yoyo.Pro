using System;
using System.Linq.Expressions;
using FreeSql;

namespace Yoyo.Pro.DbStore.FreeSqlStore.Repositories
{
    public class FSqlPartitionRepository<TEntity, TKey> :
            BaseRepository<TEntity, TKey>
            where TEntity : class
    {
        public FSqlPartitionRepository(
            IFreeSql fsql,
            Expression<Func<TEntity, bool>> filter,
            Func<string, string> asTable)
            : base(fsql, filter, asTable)
        {

        }


        public FSqlPartitionRepository(
           IFreeSql fsql,
           IUnitOfWork unitOfWork,
           Expression<Func<TEntity, bool>> filter,
           Func<string, string> asTable)
           : this(fsql, filter, asTable)
        {
            this.UnitOfWork = unitOfWork;
        }
    }
}
