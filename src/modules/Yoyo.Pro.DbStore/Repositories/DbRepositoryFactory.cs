using System;

namespace Yoyo.Pro.DbStore.Repositories
{
    public class DbRepositoryFactory : IDbRepositoryFactory
    {
        protected readonly IServiceProvider _serviceProvider;
        protected readonly IDbRepositoryDict _repositoryDict;

        public DbRepositoryFactory(IServiceProvider serviceProvider, IDbRepositoryDict repositoryDict)
        {
            this._serviceProvider = serviceProvider;
            this._repositoryDict = repositoryDict;
        }


        public virtual IDbRepository<TEntity, TPrimaryKey> GetDbRepository<TEntity, TPrimaryKey>(string key)
            where TEntity : class, IDbEntity<TPrimaryKey>
        {
            if (!_repositoryDict.TryGet(key, out Type repositoryType))
            {
                throw new ArgumentNullException("The repository-type  for the key value cannot be found");
            }

            var constructedRepositoryType = repositoryType.MakeGenericType(typeof(TEntity), typeof(TPrimaryKey));

            var result = _serviceProvider.GetService(constructedRepositoryType);

            return (IDbRepository<TEntity, TPrimaryKey>)result;
        }
    }
}
