// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Domain.Uow;
using Abp.EntityFrameworkCore;
using Abp.Extensions;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.Pro
{
    public interface IDatabaseChecker<TDbContext> : IDatabaseChecker
        where TDbContext : DbContext
    {

    }


    public class DatabaseChecker<TDbContext> : IDatabaseChecker<TDbContext>
         where TDbContext : DbContext
    {
        private readonly IDbContextProvider<TDbContext> _dbContextProvider;
        private readonly IUnitOfWorkManager _unitOfWorkManager;

        public DatabaseChecker(
            IDbContextProvider<TDbContext> dbContextProvider,
            IUnitOfWorkManager unitOfWorkManager
        )
        {
            _dbContextProvider = dbContextProvider;
            _unitOfWorkManager = unitOfWorkManager;
        }

        public bool Exist(string connectionString)
        {
            if (connectionString.IsNullOrEmpty())
            {
                //单元测试下连接字符串为空
                return true;
            }

            try
            {
                using (var uow = _unitOfWorkManager.Begin())
                {
                    // 在单租户模式下切换到host
                    using (_unitOfWorkManager.Current.SetTenantId(null))
                    {
                        _dbContextProvider.GetDbContext().Database.OpenConnection();
                        uow.Complete();
                    }
                }
            }
            catch
            {
                return false;
            }

            return true;
        }

        public virtual DbContext GetDbContext()
        {
            return _dbContextProvider.GetDbContext();
        }
    }
}
