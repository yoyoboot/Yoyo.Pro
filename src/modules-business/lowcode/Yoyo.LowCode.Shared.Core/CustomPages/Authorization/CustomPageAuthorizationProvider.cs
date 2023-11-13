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
    ///     See <see cref="CustomPagePermissions" /> for all permission names. BaseCustomPage
    /// </summary>
    public class CustomPageAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public CustomPageAuthorizationProvider()
        {
        }

        public CustomPageAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public CustomPageAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 在这里配置了BaseCustomPage 的权限。
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ??
                        context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var administration = pages.Children.FirstOrDefault(p => p.Name == LowCodePermissions.Pages_LowCode) ??
                                 pages.CreateChildPermission(LowCodePermissions.Pages_LowCode, L("LowCode"));

            var baseCustomPage =
                administration.CreateChildPermission(CustomPagePermissions.BaseCustomPage_Node,
                    L("BaseCustomPage"));
            baseCustomPage.CreateChildPermission(CustomPagePermissions.BaseCustomPage_Query,
                L("QueryBaseCustomPage"));
            baseCustomPage.CreateChildPermission(CustomPagePermissions.BaseCustomPage_Create,
                L("CreateBaseCustomPage"));
            baseCustomPage.CreateChildPermission(CustomPagePermissions.BaseCustomPage_Edit,
                L("EditBaseCustomPage"));
            baseCustomPage.CreateChildPermission(CustomPagePermissions.BaseCustomPage_Delete,
                L("DeleteBaseCustomPage"));
            baseCustomPage.CreateChildPermission(CustomPagePermissions.BaseCustomPage_BatchDelete,
                L("BatchDeleteBaseCustomPage"));
            // baseCustomPage.CreateChildPermission(CustomPagePermissions.BaseCustomPage_ExportExcel,
            //     L("ExportToExcel"));

            //// custom codes

            //// custom codes end
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LowCodeConsts.LocalizationSourceName);
        }
    }
}
