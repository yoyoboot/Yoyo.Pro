// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace Yoyo.LowCode.UserQueryModule.UserQueryDefinition
{
    /// <summary>
    /// 用户查询输入的参数
    /// </summary>
    [NotMapped]
    public class BaseUserQueryParamter
    {
        /// <summary>
        /// 此字段提供给追溯使用，不进行映射使用
        /// </summary>
        public string Id => this.Name;

        /// <summary>
        /// 参数名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 参数类型
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 参数值
        /// </summary>
        public string Val { get; set; }

        /// <summary>
        /// 获取有值的类型
        /// </summary>
        /// <returns></returns>
        public object GetValForType()
        {
            return ConvertValForType(this.Type, this.Val);
        }

        /// <summary>
        /// 根据类型转换值
        /// </summary>
        /// <param name="type"></param>
        /// <param name="val"></param>
        /// <returns></returns>
        public static object ConvertValForType(string type, object val)
        {
            if (val is string valStr && type != UserQueryConsts.UserQueryParamterType.String)
            {
                switch (type)
                {
                    case UserQueryConsts.UserQueryParamterType.Boolean:
                        return bool.Parse(valStr?.ToLower()?.Trim());

                    case UserQueryConsts.UserQueryParamterType.Decimal:
                        return decimal.Parse(valStr?.Trim());

                    case UserQueryConsts.UserQueryParamterType.Float:
                        return double.Parse(valStr?.Trim());

                    case UserQueryConsts.UserQueryParamterType.Integer:
                        return long.Parse(valStr?.Trim());

                    case UserQueryConsts.UserQueryParamterType.Timestamp:
                        return DateTime.Parse(valStr?.Trim());
                }
            }

            return val;
        }
    }
}
