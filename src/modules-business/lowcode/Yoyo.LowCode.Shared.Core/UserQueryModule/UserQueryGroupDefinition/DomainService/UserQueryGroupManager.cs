// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Yoyo.LowCode.UserQueryModule.UserQueryRelationalDefinition.DomainService;
using Yoyo.Pro.Domain;

namespace Yoyo.LowCode.UserQueryModule.UserQueryGroupDefinition.DomainService
{
    /// <inheritdoc/>
    public class UserQueryGroupManager : BasicDomainService<BaseUserQueryGroup, Guid>, IUserQueryGroupManager
    {
        #region Private Fields

        // 和组关联的QueryItem的manager
        private readonly IUserQueryGroupEntityManager _userQueryGroupItemManager;

        // 和组关联的子组 联系表
        private readonly IUserQueryGroupSubManager _userQueryGroupSubManager;

        #endregion Private Fields

        #region Public Constructors

        /// <inheritdoc/>
        public UserQueryGroupManager(IServiceProvider serviceProvider, IUserQueryGroupSubManager userQueryGroupSubManager,
            IUserQueryGroupEntityManager userQueryGroupItemManager)
            : base(serviceProvider)
        {
            _userQueryGroupSubManager = userQueryGroupSubManager;
            _userQueryGroupItemManager = userQueryGroupItemManager;
            LocalizationSourceName = LowCodeConsts.LocalizationSourceName;
        }

        #endregion Public Constructors

        #region Public Methods

        /// <inheritdoc/>
        public async Task DeleteNdoAndEntry(Guid inputId)
        {
            // 先删除联系
            await _userQueryGroupItemManager.DeleteGroupItemByGroupId(inputId);
            await _userQueryGroupSubManager.DeleteSubGroupByGroupId(inputId);
        }

        #endregion Public Methods
    }
}
