// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Abp.Application.Services.Dto;
using Abp.Domain.Entities;
using Abp.Domain.Entities.Auditing;
using Yoyo.Pro.Entities.Auditing;

namespace Yoyo.Pro.Extensions
{
    public static class EntitiesExtensions
    {
        /// <summary>
        /// 置空实体对象的审计信息
        /// </summary>
        /// <param name="entity"></param>
        public static void ResetAuditing<TPrimaryKey>(this IEntity<TPrimaryKey> entity)
        {
            if (entity == null)
            {
                return;
            }

            if (entity is ICreationAudited creationAudited)
            {
                creationAudited.CreationTime = default;
                creationAudited.CreatorUserId = null;
            }

            if (entity is ICreationNameAudited creationNameAudited)
            {
                creationNameAudited.CreatorUserName = null;
            }

            if (entity is IModificationAudited modificationAudited)
            {
                modificationAudited.LastModificationTime = default;
                modificationAudited.LastModifierUserId = null;
            }

            if (entity is IModificationNameAudited modificationNameAudited)
            {
                modificationNameAudited.LastModifierUserName = null;
            }

            if (entity is IDeletionAudited deletionAudited)
            {
                deletionAudited.DeletionTime = default;
                deletionAudited.DeleterUserId = null;
            }

            if (entity is IDeletionNameAudited deletionNameAudited)
            {
                deletionNameAudited.DeleterUserName = null;
            }
        }

        /// <summary>
        /// 置空Dto对象的审计信息
        /// </summary>
        /// <param name="entity"></param>
        public static void ResetAuditing<TPrimaryKey>(this IEntityDto<TPrimaryKey> entity)
        {
            if (entity == null)
            {
                return;
            }

            if (entity is ICreationAudited creationAudited)
            {
                creationAudited.CreationTime = default;
                creationAudited.CreatorUserId = null;
            }

            if (entity is ICreationNameAudited creationNameAudited)
            {
                creationNameAudited.CreatorUserName = null;
            }

            if (entity is IModificationAudited modificationAudited)
            {
                modificationAudited.LastModificationTime = default;
                modificationAudited.LastModifierUserId = null;
            }

            if (entity is IModificationNameAudited modificationNameAudited)
            {
                modificationNameAudited.LastModifierUserName = null;
            }

            if (entity is IDeletionAudited deletionAudited)
            {
                deletionAudited.DeletionTime = default;
                deletionAudited.DeleterUserId = null;
            }

            if (entity is IDeletionNameAudited deletionNameAudited)
            {
                deletionNameAudited.DeleterUserName = null;
            }
        }
    }
}
