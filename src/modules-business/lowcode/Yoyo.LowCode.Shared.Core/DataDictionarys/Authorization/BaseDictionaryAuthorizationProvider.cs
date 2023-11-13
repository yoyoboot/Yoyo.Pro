// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Yoyo.LowCode.Authorization;
using Yoyo.Pro.Authorization;

namespace Yoyo.LowCode.DataDictionarys.Authorization
{
    /// <summary>
    ///     权限配置都在这里。
    ///     给权限默认设置服务
    ///     See <see cref="BaseDictionaryPermissions" /> for all permission names. BaseDictionary
    /// </summary>
    public class BaseDictionaryAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public BaseDictionaryAuthorizationProvider()
        {
        }

        public BaseDictionaryAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public BaseDictionaryAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 在这里配置了BaseDictionary 的权限。
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ??
                        context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var administration = pages.Children.FirstOrDefault(p => p.Name == LowCodePermissions.Pages_LowCode) ??
                                 pages.CreateChildPermission(LowCodePermissions.Pages_LowCode, L("LowCode"));
            var dataCenter = administration.Children.FirstOrDefault(p => p.Name == DbLinkPermissions.BaseDbCenter_Node)
                             ?? administration.CreateChildPermission(DbLinkPermissions.BaseDbCenter_Node, L("BaseDbCenter_Node"));
            var BaseDictionary = dataCenter.CreateChildPermission(BaseDictionaryPermissions.BaseDictionary_Node, L("BaseDictionarys"));
            BaseDictionary.CreateChildPermission(BaseDictionaryPermissions.BaseDictionary_Query,
                L("QueryBaseDictionary"));
            BaseDictionary.CreateChildPermission(BaseDictionaryPermissions.BaseDictionary_Create,
                L("CreateBaseDictionary"));
            BaseDictionary.CreateChildPermission(BaseDictionaryPermissions.BaseDictionary_Edit,
                L("EditBaseDictionary"));
            BaseDictionary.CreateChildPermission(BaseDictionaryPermissions.BaseDictionary_Delete,
                L("DeleteBaseDictionary"));
            BaseDictionary.CreateChildPermission(BaseDictionaryPermissions.BaseDictionary_BatchDelete,
                L("BatchDeleteBaseDictionary"));

            //// custom codes

            //// custom codes end
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LowCodeConsts.LocalizationSourceName);
        }
    }
}
