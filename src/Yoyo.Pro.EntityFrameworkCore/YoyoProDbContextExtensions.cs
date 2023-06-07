// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Abp.EntityFramework;
using Abp.EntityFrameworkCore.Configuration;
using Abp.EntityFrameworkCore.Uow;
using Abp.Modules;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.DependencyInjection;
using ShardingCore.Bootstrappers;

namespace Yoyo.Pro
{
    public static class YoyoProDbContextExtensions
    {
        /// <summary>
        /// 注册 DbContext
        /// </summary>
        /// <typeparam name="TDbContext"></typeparam>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IServiceCollection AddYoyoProDbContext<TDbContext>(this IServiceCollection services)
            where TDbContext : DbContext
        {
            services.AddTransient<DbContextOptions<TDbContext>>((serviceProvider) =>
            {
                // 匹配
                var dbContextTypeMatcher = serviceProvider.GetService<IDbContextTypeMatcher>();
                var concreteDbContextType = dbContextTypeMatcher.GetConcreteType(typeof(TDbContext));
                var connectionStringResolveArgs = new ConnectionStringResolveArgs()
                {
                    ["DbContextType"] = typeof(TDbContext),
                    ["DbContextConcreteType"] = concreteDbContextType
                };

                // 获取连接字符串
                var connectionStringResolver = serviceProvider.GetService<IConnectionStringResolver>();
                var connectionString = connectionStringResolver.GetNameOrConnectionString(connectionStringResolveArgs);

                // 获取当前连接
                var existingConnection = default(DbConnection);
                var efCoreTransactionStrategy = serviceProvider
                    .GetService<IEfCoreTransactionStrategy>();
                var property = efCoreTransactionStrategy.GetType()
                .GetProperty("ActiveTransactions", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Instance);
                if (property?.GetValue(efCoreTransactionStrategy) is IDictionary<string, ActiveTransactionInfo> activeTransactions)
                {
                    if (activeTransactions.TryGetValue(connectionString, out var activeTransactionInfo))
                    {
                        existingConnection = activeTransactionInfo.DbContextTransaction
                                    .GetDbTransaction()
                                    .Connection;
                    }
                }

                // 连接信息
                var configuration = new AbpDbContextConfiguration<TDbContext>(
                    connectionString,
                    existingConnection
                    );
                configuration.DbContextOptions.UseApplicationServiceProvider(serviceProvider);

                // 配置信息
                var configurer = serviceProvider
                    .GetService<IAbpDbContextConfigurer<TDbContext>>();
                configurer.Configure(configuration);

                return configuration.DbContextOptions.Options;
            });

            return services;
        }
    }
}
