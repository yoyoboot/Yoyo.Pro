// ReSharper disable once CheckNamespace
using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Yoyo.Pro.Authorization;

// ReSharper disable once RedundantUsingDirective
namespace Yoyo.LowCode
{
    /// <summary>
    ///     权限配置都在这里。
    ///     给权限默认设置服务
    /// </summary>
    public class DynamicMenuAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public DynamicMenuAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public DynamicMenuAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 在这里配置了Document 的权限。
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ??
                        context.CreatePermission(AppPermissions.Pages, L("Pages"));

            var webFunctionModule = pages.Children.FirstOrDefault(p => p.Name == LowCodePermissions.Pages_LowCode) ??
                                    pages.CreateChildPermission(LowCodePermissions.Pages_LowCode, L("LowCode"));

            var menus = webFunctionModule.CreateChildPermission(
                DynamicMenuPermissions.Pages_Administration_DynamicMenus, L("DynamicMenus"));
            menus.CreateChildPermission(DynamicMenuPermissions.Pages_Administration_DynamicMenus_Create,
                L("CreateDynamicMenus"));
            menus.CreateChildPermission(DynamicMenuPermissions.Pages_Administration_DynamicMenus_Edit,
                L("EditDynamicMenus"));
            menus.CreateChildPermission(DynamicMenuPermissions.Pages_Administration_DynamicMenus_Delete,
                L("DeleteDynamicMenus"));
            menus.CreateChildPermission(DynamicMenuPermissions.Pages_Administration_DynamicMenus_System,
                L("SystemMenus"));
        }

        private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LowCodeConsts.LocalizationSourceName);
        }
    }
}
