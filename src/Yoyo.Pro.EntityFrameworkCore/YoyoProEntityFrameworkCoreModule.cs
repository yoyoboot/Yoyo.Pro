using Abp.Domain.Repositories;
using Abp.EntityFrameworkCore.Configuration;
using Abp.Modules;
using Abp.Zero.EntityFrameworkCore;
using Castle.MicroKernel.Registration;
using Yoyo.Pro.Common;
using Yoyo.Pro.Configuration;
using Yoyo.Pro.Repositories;

namespace Yoyo.Pro
{


    [DependsOn(
        typeof(YoyoProCoreModule),
        typeof(AbpZeroCoreEntityFrameworkCoreModule)
        )]
    public class YoyoProEntityFrameworkCoreModule : AbpModule
    {

        public override void PreInitialize()
        {
            //// 替换 IAbpEfCoreConfiguration 实现
            //if (!IocManager.IsRegistered<IYoyoProEfCoreConfiguration>()
            //    && IocManager.IsRegistered<IAbpEfCoreConfiguration>())
            //{
            //    IocManager.IocContainer.Register(
            //           Component.For<IYoyoProEfCoreConfiguration>()
            //               .ImplementedBy<YoyoProEfCoreConfiguration>()
            //               .LifestyleSingleton()
            //               .IsDefault()
            //       );

            //    IocManager.IocContainer.Register(
            //            Component.For<IAbpEfCoreConfiguration>()
            //            .UsingFactoryMethod(kernel => kernel.Resolve<IYoyoProEfCoreConfiguration>())
            //            .LifestyleSingleton()
            //            .IsDefault()
            //        );
            //}

            //// 注册 DbContext DbStorage
            //if (!IocManager.IsRegistered<IYoyoProDbContextTypeStorage>())
            //{
            //    IocManager.IocContainer.Register(
            //        Component.For<IYoyoProDbContextTypeStorage>()
            //        .ImplementedBy(typeof(YoyoProDbContextTypeStorage))
            //        .LifestyleSingleton()
            //    );
            //}

            //// 注册 DbContext Provider
            //if (!IocManager.IsRegistered<IYoyoProDbContextProvider>())
            //{
            //    IocManager.IocContainer.Register(
            //            Component.For<IYoyoProDbContextProvider>()
            //                .ImplementedBy<YoyoProDbContextProvider>()
            //                .LifestyleTransient()
            //        );
            //}

            //// 泛型仓储
            //if (!IocManager.IsRegistered(typeof(IRepository<>)))
            //{
            //    IocManager.IocContainer.Register(
            //        Component.For(typeof(IRepository<>))
            //            .ImplementedBy(typeof(YoyoProEfCoreRepository<>))
            //            .LifestyleTransient()
            //            .IsDefault()
            //    );
            //}
            //if (!IocManager.IsRegistered(typeof(IRepository<,>)))
            //{
            //    IocManager.IocContainer.Register(
            //        Component.For(typeof(IRepository<,>))
            //            .ImplementedBy(typeof(YoyoProEfCoreRepository<,>))
            //            .LifestyleTransient()
            //            .IsDefault()
            //    );
            //}
        }


        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(this.GetType().Assembly);

            IocManager.Register(
                typeof(IDatabaseChecker<>),
                typeof(DatabaseChecker<>),
                Abp.Dependency.DependencyLifeStyle.Transient
                );

        }

        public override void PostInitialize()
        {
            IocManager.InitShardingCore();
        }


    }
}
