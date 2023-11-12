// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Abp.Domain.Entities;

namespace Yoyo.LowCode.Entities
{
    public static class LowCodeEntityExtensions
    {
        /// <summary>
        /// GUID 类型
        /// </summary>
        private static Type Type_Guid { get; } = typeof(Guid);

        /// <summary>
        /// 实体缓存
        /// </summary>
        private static Dictionary<Type, bool> EntityTypeCache { get; } = new Dictionary<Type, bool>();

        /// <summary>
        /// GUID类型实体
        /// </summary>
        private static Dictionary<Type, bool> GuidEntityTypeCache { get; } = new Dictionary<Type, bool>();

        /// <summary>
        /// 是否为数据库实体类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEntity(this Type type)
        {
            return IsEntity(type, out var keyType);
        }

        /// <summary>
        /// 是否为数据库实体类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="keyType"></param>
        /// <returns></returns>
        public static bool IsEntity(this Type type, out Type keyType)
        {
            keyType = null;
            if (EntityTypeCache.TryGetValue(type, out var res))
            {
                return res;
            }

            var idProp = type.GetProperty("Id");
            keyType = idProp?.PropertyType;
            if (idProp == null)
            {
                return false;
            }

            res = typeof(IEntity<>).MakeGenericType(idProp.PropertyType).IsAssignableFrom(type);
            EntityTypeCache.Add(type, res);

            return res;
        }

        /// <summary>
        /// 是否为guid类型的数据库实体类型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsGuidEntity(this Type type)
        {
            if (GuidEntityTypeCache.TryGetValue(type, out var res))
            {
                return res;
            }

            if (!IsEntity(type, out var keyType))
            {
                GuidEntityTypeCache.Add(type, false);
                return false;
            }

            res = keyType == Type_Guid;
            GuidEntityTypeCache.Add(type, res);
            return res;
        }
    }
}
