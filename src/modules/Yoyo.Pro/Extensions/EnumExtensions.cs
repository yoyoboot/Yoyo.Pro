// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Yoyo.Pro.Extensions
{

    /// <summary>
    ///     枚举<see cref="Enum" />的扩展辅助操作方法
    /// </summary>
    public static class EnumExtensions
    {
        /// <summary>
        ///     获取枚举项上的<see cref="DescriptionAttribute" />特性的文字描述
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToDescription(this Enum value)
        {
            var type = value.GetType();
            var member = type.GetMember(value.ToString()).FirstOrDefault();
            return member != null ? member.ToDescription() : value.ToString();
        }

        /// <summary>
        ///     枚举遍历，返回枚举的名称、值、特性
        /// </summary>
        /// <param name="enumType">枚举类型</param>
        /// <param name="action">回调函数</param>
        public static void Each(this Type enumType, Action<string, string, string> action)
        {
            if (enumType.BaseType != typeof(Enum))
            {
                return;
            }
            var arr = Enum.GetValues(enumType);
            foreach (var name in arr)
            {
                var value = Convert.ToInt32(Enum.Parse(enumType, name.ToString()));
                var fieldInfo = enumType.GetField(name.ToString());
                var description = "";
                if (fieldInfo != null)
                {
                    var attr = Attribute.GetCustomAttribute(fieldInfo,
                        typeof(DescriptionAttribute), false) as DescriptionAttribute;
                    if (attr != null)
                    {
                        description = attr.Description;
                    }
                }
                action(name.ToString(), value.ToString(), description);
            }
        }

        /// <summary>
        ///     根据枚举类型值返回枚举定义Description属性
        /// </summary>
        /// <param name="value"></param>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public static string ToEnumDescriptionString(this short value, Type enumType)
        {
            var nvc = new NameValueCollection();
            var typeDescription = typeof(DescriptionAttribute);
            var fields = enumType.GetFields();
            var strText = string.Empty;
            var strValue = string.Empty;
            foreach (var field in fields)
            {
                if (field.FieldType.IsEnum)
                {
                    strValue =
                        ((int)enumType.InvokeMember(field.Name, BindingFlags.GetField, null, null, null)).ToString();
                    var arr = field.GetCustomAttributes(typeDescription, true);
                    if (arr.Length > 0)
                    {
                        var aa = (DescriptionAttribute)arr[0];
                        strText = aa.Description;
                    }
                    else
                    {
                        strText = "";
                    }
                    nvc.Add(strValue, strText);
                }
            }
            return nvc[value.ToString()];
        }



        #region 封装的方法

        /// <summary>
        ///     通用的获取枚举类型的信息
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        private static List<KeyValuePair<string, string>> GetEnumTypeList<T>()
        {
            var items = new List<KeyValuePair<string, string>>();

            typeof(T).Each(
                (name, value, description) => { items.Add(new KeyValuePair<string, string>(description, value)); });

            return items;
        }



        #endregion


        /// <summary>
        /// 获取枚举string类型的key value
        /// (Description 为key，Name为 value)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GetEnumTypeDescriptionNameList<T>()
        {
            var items = new List<KeyValuePair<string, string>>();

            typeof(T).Each(
                (name, value, description) => { items.Add(new KeyValuePair<string, string>(description, name)); });

            return items;
        }

        /// <summary>
        /// 获取枚举string类型的key value
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GetStringKeyValueList<TModel>()
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

        /// <summary>
        /// 获取string,string
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static List<KeyValuePair<string, string>> GetEntityDoubleStringKeyValueList<TModel>()
        {
            var dataList = GetEnumTypeList<TModel>();
            var result = new List<KeyValuePair<string, string>>();

            foreach (var item in dataList)
            {

                result.Add(new KeyValuePair<string, string>(item.Key, item.Value));

            }

            return result;
        }

        /// <summary>
        /// 获取string,int
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static List<KeyValuePair<string, int>> GetEntityStringIntKeyValueList<TModel>()
        {
            var dataList = GetEnumTypeList<TModel>();

            var result = new List<KeyValuePair<string, int>>();

            foreach (var item in dataList)
            {

                result.Add(new KeyValuePair<string, int>(item.Key, Convert.ToInt32(item.Value)));

            }

            return result;



        }

        /// <summary>
        /// 获取 int,int
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        public static List<KeyValuePair<int, int>> GetEntityDoubleIntKeyValueList<TModel>()
        {
            var dataList = GetEnumTypeList<TModel>();

            var result = new List<KeyValuePair<int, int>>();

            foreach (var item in dataList)
            {

                result.Add(new KeyValuePair<int, int>(Convert.ToInt32(item.Key), Convert.ToInt32(item.Value)));

            }

            return result;
        }
    }

}

