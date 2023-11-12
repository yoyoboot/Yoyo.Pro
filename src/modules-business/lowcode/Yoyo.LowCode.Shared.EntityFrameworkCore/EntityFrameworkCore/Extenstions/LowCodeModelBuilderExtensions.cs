// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Yoyo.LowCode.Entities;
using Yoyo.Pro.Entities;
using Yoyo.Pro.Entities.Auditing;
using Yoyo.Pro.Extensions;

namespace Yoyo.LowCode.EntityFrameworkCore.Extenstions
{
    public static class LowCodeModelBuilderExtensions
    {
        /// <summary>
        /// 将Id字段类型为Guid的表都设置为 <see cref="ValueGenerated.Never"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ProcessIdGeneratedNever(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!entityType.ClrType.IsGuidEntity())
                {
                    continue;
                }
                modelBuilder.Entity(entityType.ClrType)
                    .Property<Guid>("Id").ValueGeneratedNever().HasMaxLength(36);
            }

            return modelBuilder;
        }

        /// <summary>
        /// 配置Guid类型字段
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static void ConfigGuidField(this ModelBuilder modelBuilder)
        {
            foreach (var mutableEntity in modelBuilder.Model.GetEntityTypes())
            {
                // guid主键
                var properties = mutableEntity.GetProperties()
                        .Where(p => p.ClrType == typeof(Guid) ||
                               p.ClrType == typeof(Guid?));

                modelBuilder.Entity(mutableEntity.ClrType, (b) =>
                {
                    foreach (var property in properties)
                    {
                        b.Property(property.Name).HasConversion<string>().HasColumnType("RAW(36)");
                    }
                });
            }
        }

        /// <summary>
        /// 设置modelBuilder中所有表的字符串列数据最大长度
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="maxLength">最大长度，默认2000</param>
        /// <returns></returns>
        public static ModelBuilder ProcessStringMaxLength(this ModelBuilder modelBuilder, int maxLength = 2000)
        {
            // 字符串类型字段设置最大长度为2000
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                modelBuilder.ProcessStringMaxLength(entityType.ClrType, maxLength);
            }
            return modelBuilder;
        }

        /// <summary>
        /// 将指定表的字符串列数据最大长度
        /// </summary>
        /// <typeparam name="TEntity">指定表类型</typeparam>
        /// <param name="modelBuilder"></param>
        /// <param name="maxLength">最大长度，默认2000</param>
        /// <returns></returns>
        public static ModelBuilder ProcessStringMaxLength<TEntity>(this ModelBuilder modelBuilder, int maxLength = 2000)
        {
            return modelBuilder.ProcessStringMaxLength(typeof(TEntity), maxLength);
        }

        /// <summary>
        /// 将指定表的字符串列数据最大长度
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="entityType">指定表类型</param>
        /// <param name="maxLength">最大长度，默认2000</param>
        /// <returns></returns>
        public static ModelBuilder ProcessStringMaxLength(this ModelBuilder modelBuilder, Type entityType, int maxLength = 2000)
        {
            if (!entityType.IsEntity())
            {
                return modelBuilder;
            }

            if (maxLength > 2000)
            {
                maxLength = 2000;
            }
            if (maxLength < 1)
            {
                maxLength = 1;
            }

            EntityTypeBuilder entityBuilder = null;
            var props = entityType.GetProperties();
            foreach (var prop in props)
            {
                if (entityBuilder == null)
                {
                    entityBuilder = modelBuilder.Entity(entityType);
                }

                var propBuilder = entityBuilder.Property(prop.PropertyType, prop.Name);
                var currentMaxLength = propBuilder.Metadata.GetMaxLength();
                if (!currentMaxLength.HasValue || currentMaxLength > 2000)
                {
                    entityBuilder.Property(prop.PropertyType, prop.Name).HasMaxLength(maxLength);
                }
            }

            return modelBuilder;
        }

        /// <summary>
        /// 使用Oracle的映射规则
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder UseOracleTableMapping(this ModelBuilder modelBuilder)
        {
            var verifyingEntityType = new Func<IMutableEntityType, bool>((e) =>
            {
                return e.ClrType.IsEntity();
            });

            return modelBuilder
                .TableMappingToOracle(verifyingEntityType)
                .MapDiscriminators(verifyingEntityType);
        }

        /// <summary>
        /// 配置所有实现了 ICreationNameAudited/IDeletionNameAudited/IModificationNameAudited 对应的审计字段长度
        /// </summary>
        /// <param name="modelBuilder"></param>
        public static ModelBuilder AuditEntityProcessing(this ModelBuilder modelBuilder)
        {
            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (!entityType.ClrType.IsEntity())
                {
                    continue;
                }

                if (entityType.ClrType.HasInterface<ICreationNameAudited>())
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(ICreationNameAudited.CreatorUserName))
                        .HasMaxLength(512);
                }

                if (entityType.ClrType.HasInterface<IDeletionNameAudited>())
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IDeletionNameAudited.DeleterUserName))
                        .HasMaxLength(512);
                }

                if (entityType.ClrType.HasInterface<IModificationNameAudited>())
                {
                    modelBuilder.Entity(entityType.ClrType)
                        .Property(nameof(IModificationNameAudited.LastModifierUserName))
                        .HasMaxLength(512);
                }
            }

            return modelBuilder;
        }

        /// <summary>
        /// 配置配置所有实现IConcurrency实体的并发列
        /// </summary>
        /// <param name="builder"></param>
        public static ModelBuilder ConcurrencyEntityProcessing(this ModelBuilder builder)
        {
            foreach (var entityType in builder.Model.GetEntityTypes())
            {
                if (!entityType.ClrType.IsEntity())
                {
                    continue;
                }

                if (entityType.ClrType.HasInterface<IConcurrency>())
                {
                    builder.Entity(entityType.ClrType)
                        .Property(nameof(IConcurrency.ConcurrencyToken))
                        .HasMaxLength(32)
                        .IsConcurrencyToken();
                }
            }

            return builder;
        }
    }
}
