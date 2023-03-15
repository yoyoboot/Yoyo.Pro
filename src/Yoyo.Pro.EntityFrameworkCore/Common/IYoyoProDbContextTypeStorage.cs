using System;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.Pro.Common
{
    /// <summary>
    /// DbContext Type 存储器
    /// </summary>
    public interface IYoyoProDbContextTypeStorage
    {
        Type GetDbContextType(string name);

        void AddDbContextType<TDbContext>(string name)
             where TDbContext : DbContext;
    }

}
