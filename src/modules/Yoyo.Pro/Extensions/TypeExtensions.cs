using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;

namespace Yoyo.Pro.Extensions
{
    /// <summary>
    ///     类型<see cref="Type" />辅助扩展方法类
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 字符串类型
        /// </summary>
        public static readonly Type STRING_TYPE = typeof(string);

        /// <summary>
        ///     判断类型是否为Nullable类型
        /// </summary>
        /// <param name="type"> 要处理的类型 </param>
        /// <returns> 是返回True，不是返回False </returns>
        public static bool IsNullableType(this Type type)
        {
            return (type != null) && type.IsGenericType && (type.GetGenericTypeDefinition() == typeof(Nullable<>));
        }

        /// <summary>
        ///     由类型的Nullable类型返回实际类型
        /// </summary>
        /// <param name="type"> 要处理的类型对象 </param>
        /// <returns> </returns>
        public static Type GetNonNummableType(this Type type)
        {
            if (IsNullableType(type))
            {
                return type.GetGenericArguments()[0];
            }
            return type;
        }

        /// <summary>
        ///     通过类型转换器获取Nullable类型的基础类型
        /// </summary>
        /// <param name="type"> 要处理的类型对象 </param>
        /// <returns> </returns>
        public static Type GetUnNullableType(this Type type)
        {
            if (IsNullableType(type))
            {
                var nullableConverter = new NullableConverter(type);
                return nullableConverter.UnderlyingType;
            }
            return type;
        }

        /// <summary>
        ///     获取成员元数据的Description特性描述信息
        /// </summary>
        /// <param name="member">成员元数据对象</param>
        /// <param name="inherit">是否搜索成员的继承链以查找描述特性</param>
        /// <returns>返回Description特性描述信息，如不存在则返回成员的名称</returns>
        public static string ToDescription(this MemberInfo member, bool inherit = false)
        {
            var desc = member.GetAttribute<DescriptionAttribute>(inherit);
            return desc == null ? member.Name : desc.Description;
        }

        /// <summary>
        ///     检查指定指定类型成员中是否存在指定的Attribute特性
        /// </summary>
        /// <typeparam name="T">要检查的Attribute特性类型</typeparam>
        /// <param name="memberInfo">要检查的类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>是否存在</returns>
        public static bool AttributeExists<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).Any(m => m as T != null);
        }

        /// <summary>
        ///     从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>存在返回第一个，不存在返回null</returns>
        public static T GetAttribute<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            var descripts = memberInfo.GetCustomAttributes(typeof(T), inherit);
            return descripts.FirstOrDefault() as T;
        }

        /// <summary>
        ///     从类型成员获取指定Attribute特性
        /// </summary>
        /// <typeparam name="T">Attribute特性类型</typeparam>
        /// <param name="memberInfo">类型类型成员</param>
        /// <param name="inherit">是否从继承中查找</param>
        /// <returns>返回所有指定Attribute特性的数组</returns>
        public static T[] GetAttributes<T>(this MemberInfo memberInfo, bool inherit = false) where T : Attribute
        {
            return memberInfo.GetCustomAttributes(typeof(T), inherit).Cast<T>().ToArray();
        }



        /// <summary>
        ///     判断当前泛型类型是否可由指定类型的实例填充
        /// </summary>
        /// <param name="genericType">泛型类型</param>
        /// <param name="type">指定类型</param>
        /// <returns></returns>
        public static bool IsGenericAssignableFrom(this Type genericType, Type type)
        {
            genericType.CheckNotNull(nameof(genericType));
            type.CheckNotNull(nameof(type));
            if (!genericType.IsGenericType)
            {
                throw new ArgumentException("该功能只支持泛型类型的调用，非泛型类型可使用 IsAssignableFrom 方法。");
            }

            var allOthers = new List<Type> { type };
            if (genericType.IsInterface)
            {
                allOthers.AddRange(type.GetInterfaces());
            }

            foreach (var other in allOthers)
            {
                var cur = other;
                while (cur != null)
                {
                    if (cur.IsGenericType)
                    {
                        cur = cur.GetGenericTypeDefinition();
                    }
                    if (cur.IsSubclassOf(genericType) || cur == genericType)
                    {
                        return true;
                    }
                    cur = cur.BaseType;
                }
            }
            return false;
        }


        /// <summary>
        /// 判断类型是否实现此接口
        /// </summary>
        /// <typeparam name="TInterface">接口类型</typeparam>
        /// <param name="type">要判断的类型</param>
        /// <returns></returns>
        public static bool HasInterface<TInterface>(this Type type)
        {
            return type.HasInterface(typeof(TInterface));
        }

        /// <summary>
        /// 判断类型是否实现此接口
        /// </summary>
        /// <param name="type">类型</param>
        /// <param name="interfaceType">接口类型</param>
        /// <returns></returns>
        public static bool HasInterface(this Type type, Type interfaceType)
        {
            var findInterfaceType = type.GetInterface(interfaceType.FullName);
            if (findInterfaceType == null)
            {
                return false;
            }

            return findInterfaceType.FullName == interfaceType.FullName;
        }


        /// <summary>
        /// 判断是否为基本类型
        /// </summary>
        /// <param name="type"></param>
        /// <param name="skipEnum">跳过枚举</param>
        /// <returns></returns>
        public static bool IsStructs(this Type type, bool skipEnum = false)
        {
            if (skipEnum)
            {
                return (type.IsValueType || type == STRING_TYPE) && !type.IsEnum;
            }

            return type.IsValueType || type == STRING_TYPE;
        }

        /// <summary>
        /// 是否为泛型迭代器
        /// </summary>
        /// <param name="type"></param>
        /// <param name="genericType">迭代器中的泛型类型</param>
        /// <returns></returns>
        public static bool IsEnumerable(this Type type, out Type genericType)
        {
            genericType = null;
            if (type.IsGenericType)
            {
                genericType = type.GenericTypeArguments[0];
                return typeof(IEnumerable<>).MakeGenericType(type.GenericTypeArguments).IsAssignableFrom(type);
            }

            return false;
        }


        /// <summary>
        /// 是否为泛型集合
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool IsEnumerable(this Type type)
        {
            return type.IsEnumerable(out var type1);
        }

        /// <summary>
        ///     判断类型是否为集合类型
        /// </summary>
        /// <param name="type">要处理的类型</param>
        /// <returns>是返回True，不是返回False</returns>
        public static bool IsEnumerable2(this Type type)
        {
            if (type == typeof(string))
            {
                return false;
            }
            return typeof(IEnumerable).IsAssignableFrom(type);
        }

        /// <summary>
        /// 获取类型的名称，如果是泛型，则取第一个泛型参数的类型名称
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static string GetTypeFullName(this Type type)
        {
            if (type.IsGenericType)
            {
                return type.GetGenericArguments()[0].FullName;
            }
            else if (type.IsNullable(out var nullableType))
            {
                return nullableType.FullName;
            }

            return type.FullName;
        }

        public static bool IsNullable(this Type type, out Type nullableType)
        {
            nullableType = Nullable.GetUnderlyingType(type);

            return nullableType != null;
        }
    }
}
