// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;

namespace Yoyo.Pro.Reflection
{
    public class TypeFinderOptions
    {
        public Dictionary<Type, bool> SkipTypes { get; } = new Dictionary<Type, bool>();

        /// <summary>
        /// 添加跳过查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public virtual void AddSkip<T>()
            where T : class
        {
            SkipTypes[typeof(T)] = true;
        }

        /// <summary>
        /// 删除跳过查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public virtual void RemoveSkip<T>()
            where T : class
        {
            SkipTypes[typeof(T)] = false;
        }

        /// <summary>
        /// 是否跳过查找
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual bool IsSkip<T>()
            where T : class
        {
            return this.IsSkip(typeof(T));
        }

        /// <summary>
        /// 是否跳过查找
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public virtual bool IsSkip(Type type)
        {
            if (this.SkipTypes.TryGetValue(type, out bool result))
            {
                return result;
            }

            return false;
        }
    }
}
