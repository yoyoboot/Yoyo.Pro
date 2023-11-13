// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Yoyo.LowCode
{
    public static class RivenSqlServerDesignTimeServices
    {
        /// <summary>
        /// <see cref="SqlServerModelBuilderExtensions.UseIdentityColumns"/>
        /// </summary>
        /// <param name="modelBuilder"></param>
        /// <param name="seed"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static ModelBuilder UseIdentityColumnsSqlServer(this ModelBuilder modelBuilder, int seed = 1, int increment = 1)
        {
            return SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder, seed, increment);
        }

        /// <summary>
        /// <see cref="<see cref="SqlServerPropertyBuilderExtensions.UseIdentityColumn"/>"/>
        /// </summary>
        /// <param name="propertyBuilder"></param>
        /// <param name="seed"></param>
        /// <param name="increment"></param>
        /// <returns></returns>
        public static PropertyBuilder UseIdentityColumnSqlServer(
            [NotNull] this PropertyBuilder propertyBuilder,
            int seed = 1,
            int increment = 1)
        {
            return SqlServerPropertyBuilderExtensions.UseIdentityColumn(propertyBuilder, seed, increment);
        }
    }
}
