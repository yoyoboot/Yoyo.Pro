using System;
using Abp.Dependency;
using Abp.EntityFrameworkCore.Configuration;
using Castle.MicroKernel.Registration;
using Yoyo.Pro.Common;
using Microsoft.EntityFrameworkCore;

namespace Yoyo.Pro.Configuration
{
    /// <summary>
    /// YoyoPro 实现的 efcore 配置器
    /// </summary>
    public interface IYoyoProEfCoreConfiguration : IAbpEfCoreConfiguration
    {
        void AddDbContext<TDbContext>(string name, Action<AbpDbContextConfiguration<TDbContext>> action)
            where TDbContext : DbContext;
    }

    public class YoyoProEfCoreConfiguration : IYoyoProEfCoreConfiguration
    {
        private readonly IIocManager _iocManager;

        public YoyoProEfCoreConfiguration(IIocManager iocManager)
        {
            _iocManager = iocManager;
        }

        public void AddDbContext<TDbContext>(Action<AbpDbContextConfiguration<TDbContext>> action)
            where TDbContext : DbContext
        {
            this.AddDbContext(YoyoProDbContextTypeStorage.DEFAULT, action);
        }

        public void AddDbContext<TDbContext>(string name, Action<AbpDbContextConfiguration<TDbContext>> action)
           where TDbContext : DbContext
        {

            var dbContextTypeStorage = _iocManager.IocContainer.Resolve<IYoyoProDbContextTypeStorage>();
            dbContextTypeStorage.AddDbContextType<TDbContext>(name);

            _iocManager.IocContainer.Register(
                Component.For<IAbpDbContextConfigurer<TDbContext>>().Instance(
                    new AbpDbContextConfigurerAction<TDbContext>(action)
                )
            );
        }
    }

}
