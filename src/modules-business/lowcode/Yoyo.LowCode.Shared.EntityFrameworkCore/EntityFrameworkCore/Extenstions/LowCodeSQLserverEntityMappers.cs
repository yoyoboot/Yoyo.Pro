// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.CustomPages;
using Yoyo.LowCode.CustomPages.TemplateEntities;

namespace Yoyo.LowCode.EntityFrameworkCore.Extenstions
{
    public static class LowCodeSQLserverEntityMappers
    {
        public static ModelBuilder LowCodeSQLserverEntityMapper(this ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<BaseCustomPage>((builder) =>
            {
                builder.Property(o => o.ColumnConfigJson)
                    .HasColumnType("nvarchar(MAX)");
                builder.Property(o => o.FormConfigJson)
                    .HasColumnType("nvarchar(MAX)");
                builder.Property(o => o.TableConfigJson)
                    .HasColumnType("nvarchar(MAX)");
            });
            modelBuilder.Entity<BaseTemplateSample>((builder) =>
            {
                builder.Property(o => o.JSignature)
                    .HasColumnType("nvarchar(MAX)");
                builder.Property(o => o.GSignature)
                    .HasColumnType("nvarchar(MAX)");
                builder.Property(o => o.CSignature)
                    .HasColumnType("nvarchar(MAX)");
                builder.Property(o => o.KSignature)
                    .HasColumnType("nvarchar(MAX)");
                builder.Property(o => o.PSignature)
                    .HasColumnType("nvarchar(MAX)");
            });
            modelBuilder.Entity<BaseTemplateStaffAuth>((builder) =>
            {
                builder.Property(o => o.Operators)
                    .HasColumnType("nvarchar(MAX)");
                builder.Property(o => o.Inspector)
                    .HasColumnType("nvarchar(MAX)");
            });

            return modelBuilder;
        }
    }
}
