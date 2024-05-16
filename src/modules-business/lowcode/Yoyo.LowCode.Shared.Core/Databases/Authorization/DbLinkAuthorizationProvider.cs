// ReSharper disable once CheckNamespace
using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Yoyo.Pro.Authorization;

namespace Yoyo.LowCode.Authorization
{
    /// <summary>
    ///     权限配置都在这里。
    ///     给权限默认设置服务
    ///     See <see cref="DbLinkPermissions" /> for all permission names. BaseDbLink
    /// </summary>
    public class DbLinkAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public DbLinkAuthorizationProvider()
        {
        }

        public DbLinkAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public DbLinkAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 在这里配置了BaseDbLink 的权限。
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ??
                        context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var administration = pages.Children.FirstOrDefault(p => p.Name == LowCodePermissions.Pages_LowCode) ??
                                 pages.CreateChildPermission(LowCodePermissions.Pages_LowCode, L("LowCode"));

            var dataCenter = administration.Children.FirstOrDefault(p => p.Name == DbLinkPermissions.BaseDbCenter_Node)
                             ?? administration.CreateChildPermission(DbLinkPermissions.BaseDbCenter_Node, L("BaseDbCenter_Node"));

            var baseDbLink = dataCenter.CreateChildPermission(DbLinkPermissions.BaseDbLink_Node, L("BaseDbLink"));
            baseDbLink.CreateChildPermission(DbLinkPermissions.BaseDbLink_Query, L("QueryBaseDbLink"));
            baseDbLink.CreateChildPermission(DbLinkPermissions.BaseDbLink_Create, L("CreateBaseDbLink"));
            baseDbLink.CreateChildPermission(DbLinkPermissions.BaseDbLink_Edit, L("EditBaseDbLink"));
            baseDbLink.CreateChildPermission(DbLinkPermissions.BaseDbLink_Delete, L("DeleteBaseDbLink"));
            baseDbLink.CreateChildPermission(DbLinkPermissions.BaseDbLink_BatchDelete, L("BatchDeleteBaseDbLink"));

            var baseDbMoel = dataCenter.CreateChildPermission(DbModelPermissions.BaseDbModel_Node, L("BaseDbModel_Node"));
            baseDbMoel.CreateChildPermission(DbModelPermissions.BaseDbModel_Query, L("BaseDbModel_Query"));
            baseDbMoel.CreateChildPermission(DbModelPermissions.BaseDbModel_Create, L("BaseDbModel_Create"));
            baseDbMoel.CreateChildPermission(DbModelPermissions.BaseDbModel_Edit, L("BaseDbModel_Edit"));
            baseDbMoel.CreateChildPermission(DbModelPermissions.BaseDbModel_Delete, L("BaseDbModel_Delete"));
            baseDbMoel.CreateChildPermission(DbModelPermissions.BaseDbModel_OpenData, L("BaseDbModel_OpenData"));

            //// custom codes

            //// custom codes end
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LowCodeConsts.LocalizationSourceName);
        }
    }
}
