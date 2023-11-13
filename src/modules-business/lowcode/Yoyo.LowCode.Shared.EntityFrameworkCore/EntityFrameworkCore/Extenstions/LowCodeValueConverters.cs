// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Yoyo.LowCode.EntityFrameworkCore.Extenstions
{
    public static class LowCodeValueConverters
    {
        /// <summary>
        /// object - DateTime
        /// </summary>
        public static ValueConverter<object, DateTime> ObjectToDateTime = new ValueConverter<object, DateTime>(
                                    v => Convert.ToDateTime(v),
                                    v => v
                                );

        /// <summary>
        /// double - decimal
        /// </summary>
        public static ValueConverter<double, decimal> DoubleToDecimal = new ValueConverter<double, decimal>(
                                   v => Convert.ToDecimal(v),
                                   v => Convert.ToDouble(v)
                               );

        /// <summary>
        /// float - decimal
        /// </summary>
        public static ValueConverter<float, decimal> FloatToDecimal = new ValueConverter<float, decimal>(
                                   v => Convert.ToDecimal(v),
                                   v => Convert.ToSingle(v)
                               );

        /// <summary>
        /// float - decimal
        /// </summary>
        public static ValueConverter<float?, decimal> FloatNullToDecimal = new ValueConverter<float?, decimal>(
                                   v => Convert.ToDecimal(v),
                                   v => Convert.ToSingle(v)
                               );

        /// <summary>
        /// 使用值转换器(oracle only) double - decimal
        /// </summary>
        /// <param name="property"></param>
        /// <param name="defaultValue">默认值,0.0</param>
        /// <returns></returns>
        public static PropertyBuilder<double> UseConvertDoubleToDecimal(this PropertyBuilder<double> property, double? defaultValue = 0.0)
        {
            // 只有oracle会使用
            if (LowCodeDatabaseInfo.Instance.DatabaseType == DatabaseTypeEnum.Oracle)
            {
                property = property.HasConversion(DoubleToDecimal);
            }

            if (defaultValue.HasValue)
            {
                property = property.HasDefaultValue(defaultValue.Value);
            }

            return property;
        }

        /// <summary>
        /// 使用值转换器(oracle only) float - decimal
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static PropertyBuilder<float> UseConvertFloatToDecimal(this PropertyBuilder<float> property)
        {
            // 只有oracle会使用
            if (LowCodeDatabaseInfo.Instance.DatabaseType == DatabaseTypeEnum.Oracle)
            {
                property = property.HasConversion(FloatToDecimal);
            }

            return property;
        }

        /// <summary>
        /// 使用值转换器(oracle only) float - decimal
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static PropertyBuilder<float?> UseConvertFloatToDecimal(this PropertyBuilder<float?> property)
        {
            // 只有oracle会使用
            if (LowCodeDatabaseInfo.Instance.DatabaseType == DatabaseTypeEnum.Oracle)
            {
                property = property.HasConversion(FloatNullToDecimal);
            }

            return property;
        }

        /// <summary>
        /// 使用值转换器 object - DateTime
        /// </summary>
        /// <param name="property"></param>
        /// <returns></returns>
        public static PropertyBuilder<object> UseConvertObjectToDateTime(this PropertyBuilder<object> property)
        {
            property = property.HasConversion(ObjectToDateTime);

            return property;
        }

        /// <summary>
        /// 设置decimal精度，只在sqlserver下生效
        /// </summary>
        /// <param name="property"></param>
        /// <param name="integer">整数位</param>
        /// <param name="decimalPlace">小数位</param>
        /// <returns></returns>
        public static PropertyBuilder<decimal> SetDecimalPrecision(this PropertyBuilder<decimal> property, int integer, int decimalPlace)
        {
            // 只有 sqlserver 会使用
            if (LowCodeDatabaseInfo.Instance.DatabaseType == DatabaseTypeEnum.SqlServer)
            {
                return property.HasColumnType($"decimal({integer}, {decimalPlace})");
            }

            return property;
        }

        /// <summary>
        /// 设置decimal精度，只在sqlserver下生效
        /// </summary>
        /// <param name="property"></param>
        /// <param name="integer">整数位</param>
        /// <param name="decimalPlace">小数位</param>
        /// <returns></returns>
        public static PropertyBuilder<decimal?> SetDecimalPrecision(this PropertyBuilder<decimal?> property, int integer, int decimalPlace)
        {
            // 只有 sqlserver 会生效
            if (LowCodeDatabaseInfo.Instance.DatabaseType == DatabaseTypeEnum.SqlServer)
            {
                return property.HasColumnType($"decimal({integer}, {decimalPlace})");
            }

            return property;
        }
    }
}
