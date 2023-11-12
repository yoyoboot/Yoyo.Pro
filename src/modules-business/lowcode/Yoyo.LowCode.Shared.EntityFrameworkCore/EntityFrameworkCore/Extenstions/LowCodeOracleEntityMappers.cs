// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.Notifications;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.CustomPages;
using Yoyo.LowCode.CustomPages.TemplateEntities;
using Yoyo.LowCode.StagingHistory;
using Yoyo.LowCode.TemplateData.Entity;

namespace Yoyo.LowCode.EntityFrameworkCore.Extenstions
{
    public static class LowCodeOracleEntityMappers
    {
        /// <summary>
        /// 表的映射
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder LowCodeOracleEntityMapper(this ModelBuilder modelBuilder)
        {
            // 处理别名
            modelBuilder.Entity<NotificationInfo>((builder) =>
            {
                builder.Property(o => o.EntityTypeAssemblyQualifiedName)
                    .HasColumnName("EntityTypeAssemblyQualified".ToUpper());
            });
            modelBuilder.Entity<NotificationSubscriptionInfo>((builder) =>
            {
                builder.Property(o => o.EntityTypeAssemblyQualifiedName)
                    .HasColumnName("EntityTypeAssemblyQualified".ToUpper());
            });
            modelBuilder.Entity<TenantNotificationInfo>((builder) =>
            {
                builder.Property(o => o.EntityTypeAssemblyQualifiedName)
                    .HasColumnName("EntityTypeAssemblyQualified".ToUpper());
            });
            modelBuilder.Entity<BaseTemplateAllModule>((builder) =>
            {
                builder.Property(o => o.HandwritingPad)
                    .HasColumnType("CLOB");
            });
            modelBuilder.Entity<BaseCustomPage>((builder) =>
            {
                builder.Property(o => o.FormConfigJson)
                    .HasColumnType("CLOB");
                builder.Property(o => o.TableConfigJson)
                   .HasColumnType("CLOB");
                builder.Property(o => o.ColumnConfigJson)
                   .HasColumnType("CLOB");
            });
            modelBuilder.Entity<BaseTemplateData>((builder) =>
            {
                builder.Property(o => o.FormTemplateJson)
                    .HasColumnType("CLOB");
            });

            modelBuilder.Entity<BaseTemplateSample>((builder) =>
            {
                builder.Property(o => o.JSignature)
                    .HasColumnType("CLOB");
                builder.Property(o => o.GSignature)
                    .HasColumnType("CLOB");
                builder.Property(o => o.CSignature)
                    .HasColumnType("CLOB");
                builder.Property(o => o.KSignature)
                    .HasColumnType("CLOB");
                builder.Property(o => o.PSignature)
                    .HasColumnType("CLOB");
            });
            modelBuilder.Entity<BaseTemplateStaffAuth>((builder) =>
            {
                builder.Property(o => o.Operators)
                    .HasColumnType("CLOB");
                builder.Property(o => o.Inspector)
                    .HasColumnType("CLOB");
            });

            modelBuilder.Entity<BaseTemplateInspectionRecord>((builder) =>
            {
                builder.ToTable("BaseTemplateInspectird".ToUpper());
                builder.Property(o => o.ResponsibleDepartment)
                    .HasColumnName("ResponsibleDtMt".ToUpper());
            });

            //

            modelBuilder.Entity<BaseStagingHistory>((builder) =>
            {
                builder.Property(o => o.StagingJson)
                    .HasColumnType("CLOB");
            });
            return modelBuilder;
        }
    }
}
