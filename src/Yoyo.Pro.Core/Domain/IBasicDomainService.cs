using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Services;

// ReSharper disable once CheckNamespace
namespace Yoyo.Pro.Domain
{
    public interface IBasicDomainService<TEntity, TPrimaryKey> : IDomainService
    where TEntity : class, IEntity<TPrimaryKey>
    {
        /// <summary>
        ///     实体
        /// </summary>
        IRepository<TEntity, TPrimaryKey> EntityRepo { get; }

        /// <summary>
        ///     查询器
        /// </summary>
        IQueryable<TEntity> Query { get; }

        /// <summary>
        ///     查询器 - 不追踪
        /// </summary>
        IQueryable<TEntity> QueryAsNoTracking { get; }

        /// <summary>
        ///     根据id查找
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<TEntity> FindByIdAsync(TPrimaryKey id);

        /// <summary>
        ///     创建
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="createAndGetId">是否获取id</param>
        /// <returns></returns>
        Task Create(TEntity entity, bool createAndGetId = false);




        /// <summary>
        ///     批量创建
        /// </summary>
        /// <param name="entities">实体对象</param>
        /// <param name="createAndGetId">是否获取id</param>
        /// <returns></returns>
        Task Create(IEnumerable<TEntity> entities, bool createAndGetId = false);

        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Update(TEntity entity);

        /// <summary>
        ///     更新
        /// </summary>
        /// <param name="entities">实体对象</param>
        /// <returns></returns>
        Task Update(IEnumerable<TEntity> entities);

        /// <summary>
        ///     删除
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task Delete(TPrimaryKey id);

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task Delete(TEntity entity);

        /// <summary>
        ///     删除 - 按条件
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        Task Delete(Expression<Func<TEntity, bool>> predicate);

        /// <summary>
        ///     批量删除
        /// </summary>
        /// <param name="idList"></param>
        /// <returns></returns>
        Task Delete(List<TPrimaryKey> idList);

        /// <summary>
        ///     是否存在
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        Task<bool> Exist(TPrimaryKey id);

        /// <summary>
        ///     创建或更新
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        Task<TEntity> CreateOrUpdate(TEntity entity);


        /// <summary>
        ///     批量创建或更新
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        Task CreateOrUpdate(IEnumerable<TEntity> entities);
    }


    public interface IBasicDomainService<TEntity> : IBasicDomainService<TEntity, string>
        where TEntity : class, IEntity<string>
    {

    }
}
