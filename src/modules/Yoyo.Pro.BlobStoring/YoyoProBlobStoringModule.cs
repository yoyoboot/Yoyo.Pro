using Abp;
using Abp.Modules;

using Castle.MicroKernel.Registration;

using Yoyo.Pro.BlobStoring;

using System;
using System.Collections.Generic;
using System.Text;

namespace Yoyo.Pro
{
    [DependsOn(
        typeof(YoyoProKernelModule)
    )]
    public class YoyoProBlobStoringModule : AbpModule
    {

        public override void Initialize()
        {
            IocManager.RegisterAssemblyByConvention(this.GetType().Assembly);

            IocManager.IocContainer.Register(
                    Component.For(typeof(IBlobContainer<>))
                        .ImplementedBy(typeof(BlobContainer<>))
                        .LifestyleTransient()
                   );

            IocManager.IocContainer.Register(
                    Component.For<IBlobContainer>()
                        .UsingFactoryMethod(kernel =>
                        {
                            return kernel.Resolve<IBlobContainer<DefaultContainer>>();
                        })
                        .LifestyleTransient()
                );

        }
    }
}
