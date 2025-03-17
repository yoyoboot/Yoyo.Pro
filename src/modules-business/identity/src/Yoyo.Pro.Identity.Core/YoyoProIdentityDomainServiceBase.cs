// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.


using System;
using Abp.Domain.Entities;
using Abp.Domain.Services;
using Abp.Runtime.Session;
using AutoMapper;
using Yoyo.Pro.Domain;

namespace Yoyo.Pro
{

    public abstract class YoyoProIdentityDomainServiceBase : DomainService
    {
        /* Add your common members for all your domain services. */
        /*在领域服务中添加你的自定义公共方法*/

        /// <summary>
        /// AutoMapper的配置提供者
        /// </summary>
        public IConfigurationProvider MapperProvider { get; set; }

        /// <summary>
        /// AbpSession
        /// </summary>
        public IAbpSession AbpSession { get; set; }


        public YoyoProIdentityDomainServiceBase()
        {
            LocalizationSourceName = YoyoProIdentityConfigs.Localization.SourceName;
            AbpSession = NullAbpSession.Instance;
        }

    }


    public abstract class YoyoProIdentityDomainServiceBase<TEntity, TPrimaryKey> : BasicDomainService<TEntity, TPrimaryKey>
    where TEntity : class, IEntity<TPrimaryKey>
    {

        /// <summary>
        /// AutoMapper的配置提供者
        /// </summary>
        public IConfigurationProvider MapperProvider { get; set; }

        protected YoyoProIdentityDomainServiceBase(IServiceProvider serviceProvider)
            : base(serviceProvider, YoyoProIdentityConfigs.Localization.SourceName)
        {

        }
    }

    public abstract class YoyoProIdentityDomainServiceBase<TEntity> : BasicDomainService<TEntity>
        where TEntity : class, IEntity<long>
    {
        /// <summary>
        /// AutoMapper的配置提供者
        /// </summary>
        public IConfigurationProvider MapperProvider { get; set; }

        protected YoyoProIdentityDomainServiceBase(IServiceProvider serviceProvider)
            : base(serviceProvider, YoyoProIdentityConfigs.Localization.SourceName)
        {
        }
    }

}
