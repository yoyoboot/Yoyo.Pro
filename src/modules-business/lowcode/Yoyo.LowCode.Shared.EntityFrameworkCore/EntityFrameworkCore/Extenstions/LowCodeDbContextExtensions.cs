// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Abp.Json;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.UserQueryModule.UserQueryDefinition;

namespace Yoyo.LowCode.EntityFrameworkCore.Extenstions
{
    public static class LowCodeDbContextExtensions
    {
        /// <summary>
        /// 配置 LowCode 模块
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <returns></returns>
        public static ModelBuilder ConfigureLowCode(this ModelBuilder modelBuilder)
        {
            #region BaseUserQuery

            // BaseUserQuery
            modelBuilder.Entity<BaseUserQuery>((builder) =>
            {
                builder.Property(o => o.QueryParamters)
                    .HasConversion(s => s.ToJsonString(false, false),
                        t => t.FromJsonString<List<BaseUserQueryParamter>>());
            });

            #endregion BaseUserQuery

            return modelBuilder;
        }
    }
}
