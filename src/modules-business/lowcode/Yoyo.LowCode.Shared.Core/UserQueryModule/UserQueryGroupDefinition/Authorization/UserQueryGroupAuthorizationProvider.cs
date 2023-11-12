// ReSharper disable once CheckNamespace
using System.Linq;
using Abp.Authorization;
using Abp.Configuration.Startup;
using Abp.Localization;
using Yoyo.LowCode.UserQueryModule.Authorization;
using Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition;
using Yoyo.Pro.Authorization;

namespace Yoyo.LowCode.Authorization
{
    /// <summary>
    /// 权限配置都在这里。
    /// 给权限默认设置服务
    /// See <see cref="UserQueryGroupPermissions" /> 所有权限常量的定义位置
    /// <see cref="BaseUserQueryGroup"/> 实体位置
    ///前往 <see cref="MedProApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    ///</summary>
    public class UserQueryGroupAuthorizationProvider : AuthorizationProvider
    {
        private readonly bool _isMultiTenancyEnabled;

        public UserQueryGroupAuthorizationProvider()
        {
        }

        /// <summary>
        /// 多租户配置
        /// </summary>
        /// <param name="isMultiTenancyEnabled"></param>
        public UserQueryGroupAuthorizationProvider(bool isMultiTenancyEnabled)
        {
            _isMultiTenancyEnabled = isMultiTenancyEnabled;
        }

        public UserQueryGroupAuthorizationProvider(IMultiTenancyConfig multiTenancyConfig)
        {
            _isMultiTenancyEnabled = multiTenancyConfig.IsEnabled;
        }

        /// <summary>
        /// 在这里配置了的权限。
        /// </summary>
        /// <param name="context"></param>
		public override void SetPermissions(IPermissionDefinitionContext context)
        {
            // 在这里配置了 的权限。
            var pages = context.GetPermissionOrNull(AppPermissions.Pages) ?? context.CreatePermission(AppPermissions.Pages, L("Pages"));

            //var administration = pages.Children.FirstOrDefault(p => p.Name == AppPermissions.Pages_Administration) ??
            //pages.CreateChildPermission(AppPermissions.Pages_Administration, L("Administration"));

            var userQueryRoot = pages.Children.FirstOrDefault(p => p.Name == UserQueryCommonPermissions.UserQuery_Root) ?? pages.CreateChildPermission(UserQueryCommonPermissions.UserQuery_Root, L("Pages.BaseUserQuery.Root"));

            var userQueryGroup = userQueryRoot.Children.FirstOrDefault(p => p.Name == UserQueryGroupPermissions.UserQueryGroup_Node) ?? userQueryRoot.CreateChildPermission(UserQueryGroupPermissions.UserQueryGroup_Node, L("Pages.BaseUserQueryGroup.Node"));

            userQueryGroup.CreateChildPermission(UserQueryGroupPermissions.UserQueryGroup_Query, L("Query"));
            userQueryGroup.CreateChildPermission(UserQueryGroupPermissions.UserQueryGroup_Create, L("Create"));
            userQueryGroup.CreateChildPermission(UserQueryGroupPermissions.UserQueryGroup_Edit, L("Edit"));
            userQueryGroup.CreateChildPermission(UserQueryGroupPermissions.UserQueryGroup_Delete, L("Delete"));
            userQueryGroup.CreateChildPermission(UserQueryGroupPermissions.UserQueryGroup_Copy, L("Copy"));

            userQueryGroup.CreateChildPermission(UserQueryGroupPermissions.UserQueryGroup_ExportExcel, L("ExportToExcel"));
            userQueryGroup.CreateChildPermission(UserQueryGroupPermissions.UserQueryGroup_ImportExcel, L("ImportToExcel"));

            //// custom codes

            //// custom codes end
        }

        /// <summary>
        /// 多语言本地化
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
		private static ILocalizableString L(string name)
        {
            return new LocalizableString(name, LowCodeConsts.LocalizationSourceName);
        }
    }
}
