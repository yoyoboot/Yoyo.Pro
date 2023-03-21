// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Abp.Extensions;
using Yoyo.Pro.Extensions;

namespace Yoyo.Pro.EnumHelper
{
    public class EnumConvert : IEnumConvert
    {
        #region 封装的方法

        /// <summary>
        ///     通用的获取枚举类型的信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private List<KeyValuePair<string, string>> GetEnumTypeList<T>()
        {
            var items = new List<KeyValuePair<string, string>>();

            typeof(T).Each(
                (name, value, description) => { items.Add(new KeyValuePair<string, string>(description, value)); });

            return items;
        }

        #endregion 封装的方法

        public List<KeyValuePair<string, string>> GetEntityDoubleStringKeyValueList<TModel>()
        {
            var items = new List<KeyValuePair<string, string>>();

            typeof(TModel).Each(
                (name, value, description) =>
                {
                    if (description.IsNullOrWhiteSpace())
                    {
                        description = name;
                    }

                    items.Add(new KeyValuePair<string, string>(description, name));
                });

            return items;
        }

        public List<KeyValuePair<string, int>> GetEntityStringIntKeyValueList<TModel>()
        {
            var dataList = GetEnumTypeList<TModel>();

            var result = new List<KeyValuePair<string, int>>();

            foreach (var item in dataList)
            {
                result.Add(new KeyValuePair<string, int>(item.Key, Convert.ToInt32(item.Value)));
            }

            return result;
        }

        public List<KeyValuePair<int, int>> GetEntityDoubleIntKeyValueList<TModel>()
        {
            var dataList = GetEnumTypeList<TModel>();

            var result = new List<KeyValuePair<int, int>>();

            foreach (var item in dataList)
            {
                result.Add(new KeyValuePair<int, int>(Convert.ToInt32(item.Key), Convert.ToInt32(item.Value)));
            }

            return result;
        }

        public List<KeyValuePair<string, string>> GetStringKeyValueList<TModel>()
            where TModel : Enum
        {
            var result = new List<KeyValuePair<string, string>>();

            var enumType = typeof(TModel);

            var enumValues = Enum.GetValues(enumType);
            TModel[] enumValueArry = new TModel[enumValues.Length];
            enumValues.CopyTo(enumValueArry, 0);
            foreach (var item in enumValueArry)
            {
                result.Add(new KeyValuePair<string, string>(item.ToString(), item.ToString()));
            }

            return result;
        }

        public List<EnumInfo> GetEntityEnumOutputDtoList<TModel>()
        {
            var items = new List<EnumInfo>();

            typeof(TModel).Each(
                (name, value, description) =>
                {
                    if (description.IsNullOrWhiteSpace())
                    {
                        description = name;
                    }

                    items.Add(new EnumInfo()
                    {
                        Name = name,
                        Description = description,
                        Value = value
                    });
                });

            return items;
        }
    }

}
