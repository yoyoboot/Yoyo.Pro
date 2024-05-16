// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.IdentityServer4;
using Microsoft.EntityFrameworkCore;
using Yoyo.LowCode.CustomPages;
using Yoyo.LowCode.CustomPages.TemplateEntities;
using Yoyo.LowCode.Databases.Entity;
using Yoyo.LowCode.DataDictionarys;
using Yoyo.LowCode.DynamicMenus;
using Yoyo.LowCode.LowCodeAPI;
using Yoyo.LowCode.LowCodeViewModels;
using Yoyo.LowCode.StagingHistory;
using Yoyo.LowCode.TemplateData.Entity;
using Yoyo.LowCode.UserQueryModule.UserQueryDefinition;
using Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition;
using Yoyo.LowCode.UserQueryModule.UserQueryRelationalDefinition;
using Yoyo.Pro;
using Yoyo.Pro.MultiTenancy;
using Yoyo.Pro.Roles;
using Yoyo.Pro.Users;

namespace Yoyo.LowCode
{
    public abstract class LowCodeSharedDbContext<TDbContext> : YoyoProDbContext<Tenant, Role, User, TDbContext>,
        IYoyoLowCodeDbContext
        where TDbContext : LowCodeSharedDbContext<TDbContext>
    {
        /* Define a DbSet for each entity of the application */

        #region IdentityServer4

        public virtual DbSet<PersistedGrantEntity> PersistedGrants { get; set; }

        #endregion IdentityServer4

        public LowCodeSharedDbContext(DbContextOptions<TDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<BaseCustomPage> BaseCustomPage { get; set; }
        public virtual DbSet<BaseDynamicMenu> BaseDynamicMenu { get; set; }
        public virtual DbSet<BaseDbTableRelation> BaseDbTableRelation { get; set; }
        public virtual DbSet<BaseDictionaryType> BaseDictionaryType { get; set; }
        public virtual DbSet<BaseDictionaryValue> BaseDictionaryValue { get; set; }
        public virtual DbSet<BaseUserQuery> BaseUserQuery { get; set; }
        public virtual DbSet<BaseUserQueryGroup> BaseUserQueryGroup { get; set; }
        public virtual DbSet<BaseUserQueryGroupEntity> BaseUserQueryGroupEntity { get; set; }
        public virtual DbSet<BaseQueryGroupSub> BaseQueryGroupSub { get; set; }
        public virtual DbSet<BaseDbLink> BaseDbLink { get; set; }
        public virtual DbSet<BaseComFields> BaseComFields { get; set; }
        public virtual DbSet<BaseTemplateData> BaseTemplateData { get; set; }

        public virtual DbSet<BaseTemplateQuestionnaire> BaseTemplateQuestionnaire { get; set; }
        public virtual DbSet<BaseTemplateInspectionRecord> BaseTemplateInspectionRecord { get; set; }
        public virtual DbSet<BaseTemplateStaffAuth> BaseTemplateStaffAuth { get; set; }
        public virtual DbSet<BaseTemplateProduct> BaseTemplateProduct { get; set; }
        public virtual DbSet<BaseTemplateSample> BaseTemplateSample { get; set; }

        public virtual DbSet<BaseTemplateAllModule> BaseTemplateAllModule { get; set; }

        public virtual DbSet<BaseStagingHistory> BaseStagingHistory { get; set; }

        public virtual DbSet<LowCodeField> LowCodeField { get; set; }

        public virtual DbSet<LowCodeModel> LowCodeModel { get; set; }

        public virtual DbSet<LowCodeModelRelation> LowCodeModelRelation { get; set; }

        public virtual DbSet<LowCodeDefaultField> LowCodeDefaultFields { get; set; }

        public virtual DbSet<ApplyType> ApplyType { get; set; }

        public virtual DbSet<Apply> Apply { get; set; }
        public virtual DbSet<ApplyAPI> ApplyAPI { get; set; }
        public virtual DbSet<ApplyAuthentication> ApplyAuthentication { get; set; }
        public virtual DbSet<ApplyHealthTesting> ApplyHealthTesting { get; set; }
        public virtual DbSet<CommonTypeTable> CommonTypeTable { get; set; }
        public virtual DbSet<ApplyQHData> ApplyQHData { get; set; }
        public virtual DbSet<ApplyBodyData> ApplyBodyData { get; set; }

        public virtual DbSet<AgentAPI> AgentAPI { get; set; }
        public virtual DbSet<ParameterMapping> ParameterMapping { get; set; }

        public virtual DbSet<AppAuth> AppAuth { get; set; }
        public virtual DbSet<AppAuthMapping> AppAuthMapping { get; set; }

        public virtual DbSet<APIRequestRecord> APIRequestRecord { get; set; }

        public virtual DbSet<FlowControl> FlowControl { get; set; }

        public virtual DbSet<FlowControlMapping> FlowControlMapping { get; set; }
    }
}
