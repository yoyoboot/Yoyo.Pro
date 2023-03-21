// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Abp.Authorization.Roles;
using Abp.Authorization.Users;
using Abp.Domain.Entities;
using Abp.EntityFrameworkCore;
using Abp.MultiTenancy;
using Abp.Zero.EntityFrameworkCore;
using Yoyo.Pro.Entities;
using Yoyo.Pro.Entities.Auditing;
using Yoyo.Pro.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;


namespace Yoyo.Pro
{
    public abstract class YoyoProDbContextBase<TTenant, TRole, TUser, TSelf>
        : AbpZeroDbContext<TTenant, TRole, TUser, TSelf>
        where TTenant : AbpTenant<TUser>
        where TRole : AbpRole<TUser>
        where TUser : AbpUser<TUser>
        where TSelf : YoyoProDbContextBase<TTenant, TRole, TUser, TSelf>
    {

        protected YoyoProDbContextBase(DbContextOptions<TSelf> options)
            : base(options)
        {
        }


        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            this.PreModelCreating(modelBuilder);

            base.OnModelCreating(modelBuilder);

            this.ModelCreating(modelBuilder);

            this.PostModelCreating(modelBuilder);
        }

        /// <summary>
        /// 模型创建之前 - by YoyoPro
        /// </summary>
        /// <param name="modelBuilder"></param>
        public virtual void PreModelCreating(ModelBuilder modelBuilder)
        {

        }

        /// <summary>
        /// 模型配置 - by YoyoPro
        /// </summary>
        /// <param name="modelBuilder"></param>
        public abstract void ModelCreating(ModelBuilder modelBuilder);

        /// <summary>
        /// 模型创建之后 - by YoyoPro
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected virtual void PostModelCreating(ModelBuilder modelBuilder)
        {
            // 配置并发实体
            ConfigConcurrency(modelBuilder);
        }


        #region 实体并发配置，更新并发字段值

        /// <summary>
        ///     配置配置所有实现IConcurrency实体的并发列
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void ConfigConcurrency(ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (entityType.ClrType.HasInterface<IConcurrency>())
                {
                    builder.Entity(entityType.ClrType)
                        .Property(nameof(IConcurrency.ConcurrencyToken))
                        .HasMaxLength(32)
                        .IsConcurrencyToken();
                }
            }
        }

        /// <summary>
        ///     更新并发实体的的并发字段的值
        /// </summary>
        protected virtual void UpdateConcurrencyEntitys()
        {
            var entityEntries = ChangeTracker.Entries()
                .Where(o => (o.State == EntityState.Added
                             || o.State == EntityState.Modified
                             || o.State == EntityState.Deleted)
                            && o.Entity is IConcurrency
                )
                .ToList();
            foreach (var entry in entityEntries)
            {
                if (entry.Entity is IConcurrency concurrency)
                {
                    concurrency.ConcurrencyToken = Guid.NewGuid().ToString("N");
                }
            }
        }

        #endregion 实体并发配置，更新并发字段值


        #region 审计重写

        /// <summary>
        ///     重写添加
        /// </summary>
        /// <param name="entityAsObj"></param>
        /// <param name="userId"></param>
        protected override void SetCreationAuditProperties(object entityAsObj, string userId)
        {
            userId = AbpSession.UserId;
            base.SetCreationAuditProperties(entityAsObj, userId);
            if (entityAsObj is ICreationNameAudited creationNameAudited)
            {
                creationNameAudited.CreatorUserName = AbpSession.GetUserName();
            }
        }

        /// <summary>
        ///     重写修改
        /// </summary>
        /// <param name="entityAsObj"></param>
        /// <param name="userId"></param>
        protected override void SetModificationAuditProperties(object entityAsObj, string userId)
        {
            userId = AbpSession.UserId;
            base.SetModificationAuditProperties(entityAsObj, userId);
            if (entityAsObj is IModificationNameAudited modificationNameAudited)
            {
                modificationNameAudited.LastModifierUserName = AbpSession.GetUserName();
            }
        }

        /// <summary>
        ///     重写删除
        /// </summary>
        /// <param name="entityAsObj"></param>
        /// <param name="userId"></param>
        protected override void SetDeletionAuditProperties(object entityAsObj, string userId)
        {
            userId = AbpSession.UserId;
            base.SetDeletionAuditProperties(entityAsObj, userId);
            if (entityAsObj is IDeletionNameAudited deletionNameAudited)
            {
                deletionNameAudited.DeleterUserName = AbpSession.GetUserName();
            }
        }

        #endregion 审计重写

        #region 重写 SaveChanges,更新并发实体

        public override int SaveChanges()
        {
            // 并发实体更新并发列的值
            UpdateConcurrencyEntitys();

            return base.SaveChanges();
        }

        public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            // 并发实体更新并发列的值
            UpdateConcurrencyEntitys();

            return base.SaveChangesAsync(cancellationToken);
        }

        #endregion 重写 SaveChanges,更新并发实体
    }
}
