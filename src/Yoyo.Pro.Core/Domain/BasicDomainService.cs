// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Abp;
using Abp.Domain.Entities;
using Abp.Domain.Repositories;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace Yoyo.Pro.Domain
{
    public abstract class BasicDomainService<TEntity, TPrimaryKey> : DomainService, IBasicDomainService<TEntity, TPrimaryKey>
        where TEntity : class, IEntity<TPrimaryKey>
    {

        public BasicDomainService(IServiceProvider serviceProvider, string localizationSourceName = null)
        {
            ServiceProvider = serviceProvider;
            EntityRepo = serviceProvider.GetRequiredService<IRepository<TEntity, TPrimaryKey>>();
            AbpSession = serviceProvider.GetRequiredService<IAbpSession>();
            LocalizationSourceName = localizationSourceName ?? YoyoProConsts.LocalizationSourceName;
        }
        public virtual IServiceProvider ServiceProvider { get; }

        public virtual IAbpSession AbpSession { get; }

        public virtual IRepository<TEntity, TPrimaryKey> EntityRepo { get; }

        public virtual IQueryable<TEntity> Query => EntityRepo.GetAll();

        public virtual IQueryable<TEntity> QueryAsNoTracking => Query.AsNoTracking();

        public virtual async Task<TEntity> FindByIdAsync(TPrimaryKey id)
        {
            return await EntityRepo.FirstOrDefaultAsync(id);
        }

        public virtual async Task Create(TEntity entity, bool createAndGetId = false)
        {
            if (createAndGetId)
            {
                await EntityRepo.InsertAndGetIdAsync(entity);
                return;
            }


            await EntityRepo.InsertAsync(entity);

        }

        public async Task Create(IEnumerable<TEntity> entities, bool createAndGetId = false)
        {
            foreach (var entity in entities)
                await Create(entity, createAndGetId);
        }

        public virtual async Task Update(TEntity entity)
        {
            await EntityRepo.UpdateAsync(entity);
        }

        public virtual async Task Update(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                await Update(entity);
        }

        public virtual async Task Delete(TPrimaryKey id)
        {
            await EntityRepo.DeleteAsync(id);
        }

        public virtual async Task Delete(TEntity entity)
        {
            await EntityRepo.DeleteAsync(entity);
        }

        public virtual async Task Delete(List<TPrimaryKey> idList)
        {
            if (idList == null || idList.Count == 0)
                return;

            await EntityRepo.DeleteAsync(o => idList.Contains(o.Id));
        }

        public async Task Delete(Expression<Func<TEntity, bool>> predicate)
        {
            await EntityRepo.DeleteAsync(predicate);
        }

        public async Task<bool> Exist(TPrimaryKey id)
        {
            var count = await EntityRepo.CountAsync(o => o.Id.Equals(id));
            return count > 0;
        }

        public async Task<TEntity> CreateOrUpdate(TEntity entity)
        {
            return await EntityRepo.InsertOrUpdateAsync(entity);
        }

        public async Task CreateOrUpdate(IEnumerable<TEntity> entities)
        {
            foreach (var entity in entities)
                await EntityRepo.InsertOrUpdateAsync(entity);
        }

        /// <summary>
        ///     获取服务实例
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected T GetService<T>()
        {
            return ServiceProvider.GetRequiredService<T>();
        }
    }


    public abstract class BasicDomainService<TEntity> : BasicDomainService<TEntity, long>
        where TEntity : class, IEntity<long>
    {
        public BasicDomainService(IServiceProvider serviceProvider, string localizationSourceName = null)
            : base(serviceProvider, localizationSourceName)
        {
        }
    }
}
