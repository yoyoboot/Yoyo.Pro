// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Abp.IdentityServer4vNext;
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

namespace Yoyo.LowCode
{
    public interface IYoyoLowCodeDbContext : IAbpPersistedGrantDbContext
    {
        #region PageConfig

        DbSet<BaseCustomPage> BaseCustomPage { get; set; }

        DbSet<BaseDynamicMenu> BaseDynamicMenu { get; set; }

        DbSet<BaseDbTableRelation> BaseDbTableRelation { get; set; }

        DbSet<BaseDictionaryType> BaseDictionaryType { get; set; }
        DbSet<BaseDictionaryValue> BaseDictionaryValue { get; set; }
        DbSet<BaseUserQuery> BaseUserQuery { get; set; }
        DbSet<BaseUserQueryGroup> BaseUserQueryGroup { get; set; }
        DbSet<BaseUserQueryGroupEntity> BaseUserQueryGroupEntity { get; set; }
        DbSet<BaseQueryGroupSub> BaseQueryGroupSub { get; set; }

        DbSet<BaseStagingHistory> BaseStagingHistory { get; set; }

        #endregion PageConfig

        #region DataBase

        DbSet<BaseDbLink> BaseDbLink { get; set; }

        DbSet<BaseComFields> BaseComFields { get; set; }

        DbSet<BaseTemplateData> BaseTemplateData { get; set; }

        #endregion DataBase

        #region TemplateEntitys

        DbSet<BaseTemplateQuestionnaire> BaseTemplateQuestionnaire { get; set; }
        DbSet<BaseTemplateInspectionRecord> BaseTemplateInspectionRecord { get; set; }
        DbSet<BaseTemplateStaffAuth> BaseTemplateStaffAuth { get; set; }
        DbSet<BaseTemplateProduct> BaseTemplateProduct { get; set; }
        DbSet<BaseTemplateSample> BaseTemplateSample { get; set; }

        DbSet<BaseTemplateAllModule> BaseTemplateAllModule { get; set; }

        #endregion TemplateEntitys

        #region api网关

        DbSet<ApplyType> ApplyType { get; set; }

        DbSet<Apply> Apply { get; set; }

        DbSet<ApplyAPI> ApplyAPI { get; set; }
        DbSet<ApplyAuthentication> ApplyAuthentication { get; set; }
        DbSet<ApplyHealthTesting> ApplyHealthTesting { get; set; }
        DbSet<CommonTypeTable> CommonTypeTable { get; set; }
        DbSet<ApplyQHData> ApplyQHData { get; set; }
        DbSet<ApplyBodyData> ApplyBodyData { get; set; }

        DbSet<AgentAPI> AgentAPI { get; set; }
        DbSet<ParameterMapping> ParameterMapping { get; set; }

        DbSet<AppAuth> AppAuth { get; set; }
        DbSet<AppAuthMapping> AppAuthMapping { get; set; }

        DbSet<APIRequestRecord> APIRequestRecord { get; set; }

        DbSet<FlowControl> FlowControl { get; set; }

        DbSet<FlowControlMapping> FlowControlMapping { get; set; }

        #endregion api网关

        DbSet<LowCodeField> LowCodeField { get; set; }

        DbSet<LowCodeModel> LowCodeModel { get; set; }

        DbSet<LowCodeModelRelation> LowCodeModelRelation { get; set; }

        DbSet<LowCodeDefaultField> LowCodeDefaultFields { get; set; }
    }
}
