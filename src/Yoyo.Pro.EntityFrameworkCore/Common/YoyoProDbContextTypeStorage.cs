using System;
using System.Collections.Generic;
using Abp;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.Pro.Common
{
    public class YoyoProDbContextTypeStorage : IYoyoProDbContextTypeStorage
    {
        /// <summary>
        /// 默认键值
        /// </summary>
        public const string DEFAULT = "default_dbcontext";

        readonly Dictionary<string, Type> _dict;

        public YoyoProDbContextTypeStorage()
        {
            this._dict = new Dictionary<string, Type>();
        }

        public void AddDbContextType<TDbContext>(string name)
            where TDbContext : DbContext
        {
            Check.NotNullOrWhiteSpace(name, nameof(name));

            this._dict.TryAdd(name, typeof(TDbContext));
        }

        public void AddDefaultDbContextType<TDbContext>() where TDbContext : DbContext
        {
            this.AddDbContextType<TDbContext>(DEFAULT);
        }

        public Type GetDbContextType(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
            {
                name = DEFAULT;
            }

            if (this._dict.TryGetValue(name, out var res))
            {
                return res;
            }
            if (name == DEFAULT)
            {
                throw new Exception("找不到默认的的 DbContext! 请检查是否注册！");
            }
            else
            {
                throw new Exception($"找不到名为 {name} 的 DbContext! 请检查是否注册！");
            }
        }

    }

}
