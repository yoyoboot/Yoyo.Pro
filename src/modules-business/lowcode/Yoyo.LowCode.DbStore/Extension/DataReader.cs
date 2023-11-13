using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data;
using System.Reflection;

namespace Yoyo.LowCode.Extension
{
    /// <summary>
    /// DataReader
    /// </summary>
    public static class DataReader
    {
        /// <summary>
        ///  将IDataReader转换为DataTable
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static DataTable ToTable(this IDataReader reader)
        {
            using (reader)
            {
                DataTable data = new DataTable("Table");
                int intFieldCount = reader.FieldCount;
                for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
                {
                    data.Columns.Add(reader.GetName(intCounter).ToLower(), reader.GetFieldType(intCounter));
                }
                data.BeginLoadData();
                object[] objValues = new object[intFieldCount];
                while (reader.Read())
                {
                    reader.GetValues(objValues);
                    data.LoadDataRow(objValues, true);
                }
                reader.Close();
                reader.Dispose();
                data.EndLoadData();
                return data;
            }
        }

        /// <summary>
        ///  将IDataReader转换为DataTable 无改小写
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static DataTable ToTableByUnconvert(this IDataReader reader)
        {
            using (reader)
            {
                DataTable data = new DataTable("Table");
                int intFieldCount = reader.FieldCount;
                for (int intCounter = 0; intCounter < intFieldCount; ++intCounter)
                {
                    data.Columns.Add(reader.GetName(intCounter), reader.GetFieldType(intCounter));
                }
                data.BeginLoadData();
                object[] objValues = new object[intFieldCount];
                while (reader.Read())
                {
                    reader.GetValues(objValues);
                    data.LoadDataRow(objValues, true);
                }
                reader.Close();
                reader.Dispose();
                data.EndLoadData();
                return data;
            }
        }

        /// <summary>
        ///  将IDataReader转换为List
        /// </summary>
        /// <param name="reader"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this IDataReader reader)
        {
            using (reader)
            {
                List<string> field = new List<string>(reader.FieldCount);
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    //field.Add(reader.GetName(i).ToLower().Replace("f_", ""));
                    field.Add(reader.GetName(i).ToLower());
                }
                List<T> list = new List<T>();
                while (reader.Read())
                {
                    T model = Activator.CreateInstance<T>();
                    foreach (PropertyInfo property in model.GetType().GetProperties(BindingFlags.GetProperty | BindingFlags.Public | BindingFlags.Instance))
                    {
                        var columnattribute = property.GetCustomAttribute(typeof(ColumnAttribute), false);
                        //判断实体是否带Column特性
                        if (columnattribute != null)
                        {
                            var column = (ColumnAttribute)columnattribute;
                            if (field.Contains(column.Name.ToLower()))
                            {
                                if (!IsNullOrDBNull(reader[column.Name]))
                                {
                                    property.SetValue(model, HackType(reader[column.Name], property.PropertyType), null);
                                }
                            }
                        }
                        else
                        {
                            //先判断无“f_”
                            var value = field.Find(f => f.Contains(property.Name.ToLower()));
                            //未能找到
                            if (value == null)
                            {
                                //加"f_"
                                value = field.Find(f => f.Contains("f_" + property.Name.ToLower()));
                            }
                            if (value != null)
                            {
                                if (!IsNullOrDBNull(reader[value]))
                                {
                                    property.SetValue(model, HackType(reader[value], property.PropertyType), null);
                                }
                            }
                        }
                        //if (field.Contains(property.Name.ToLower()))
                        //{
                        //    if (!IsNullOrDBNull(reader["f_" + property.Name]))
                        //    {
                        //        property.SetValue(model, HackType(reader["f_" + property.Name], property.PropertyType), null);
                        //    }
                        //}
                    }
                    list.Add(model);
                }
                reader.Close();
                reader.Dispose();
                return list;
            }
        }

        private static object HackType(object value, Type conversionType)
        {
            if (conversionType.IsGenericType && conversionType.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
            {
                if (value == null)
                    return null;
                System.ComponentModel.NullableConverter nullableConverter = new System.ComponentModel.NullableConverter(conversionType);
                conversionType = nullableConverter.UnderlyingType;
            }
            return Convert.ChangeType(value, conversionType);
        }

        private static bool IsNullOrDBNull(object obj)
        {
            return ((obj is DBNull) || string.IsNullOrEmpty(obj.ToString())) ? true : false;
        }
    }
}
