// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Abp.Dependency;

namespace Yoyo.Pro.EnumHelper
{

    public interface IEnumConvert : ISingletonDependency
    {

        /// <summary>
        /// 将枚举转换为枚举类的列表
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        List<EnumInfo> GetEntityEnumOutputDtoList<TModel>();


        /// <summary>
        /// 将枚举类型的值返回为string,string的键值对
        /// </summary>
        /// <returns></returns>
        List<KeyValuePair<string, string>> GetEntityDoubleStringKeyValueList<TModel>();
        /// <summary>
        /// 获取string,int
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        List<KeyValuePair<string, int>> GetEntityStringIntKeyValueList<TModel>();
        /// <summary>
        /// 获取 int,int
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        List<KeyValuePair<int, int>> GetEntityDoubleIntKeyValueList<TModel>();

        /// <summary>
        /// 获取枚举string类型的key value
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        List<KeyValuePair<string, string>> GetStringKeyValueList<TModel>()
            where TModel : Enum;

    }

}
