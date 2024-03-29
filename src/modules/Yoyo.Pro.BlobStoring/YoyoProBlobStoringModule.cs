using Abp;
using Abp.Modules;

using Castle.MicroKernel.Registration;

using Abp.BlobStoring;

using System;
using System.Collections.Generic;
using System.Text;

namespace Yoyo.Pro
{
    [DependsOn(
        typeof(AbpBlobStoringModule)
    )]
    public class YoyoProBlobStoringModule : AbpModule
    {

        public override void Initialize()
        {

        }
    }
}
