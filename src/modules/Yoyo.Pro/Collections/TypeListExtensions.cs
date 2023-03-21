// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Abp.Collections
{
    public static class TypeListExtensions
    {
        /// <summary>
        /// Adds a type to list if it's not already in the list.
        /// </summary>
        /// <typeparam name="TBaseType">BaseType</typeparam>
        /// <typeparam name="T">Type</typeparam>
        public static bool TryAdd<TBaseType, T>(this ITypeList<TBaseType> typeList)
            where T : TBaseType
        {
            if (typeList.Contains<T>())
            {
                return false;
            }

            typeList.Add<T>();
            return true;
        }
    }
}
