// Licensed to the .NET under one or more agreements.
// The .NET licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using Yoyo.Pro.EnumHelper;
using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Yoyo.Pro.Swagger
{
    /// <summary>
    /// 枚举处理器，将会把所有的枚举信息放到 <see cref="EnumConsts.BaseEnumMaps"/> 中
    /// </summary>
    public class SwaggerEnumSchemaFilter : ISchemaFilter
    {
        static readonly Type _displayNameAttributeType = typeof(DescriptionAttribute);
        public void Apply(OpenApiSchema schema, SchemaFilterContext context)
        {
            var type = context.Type;
            if (!type.IsEnum || schema.Extensions.ContainsKey("x-enumNames"))
            {
                return;
            }

            var enumNames = new OpenApiArray();
            enumNames.AddRange(Enum.GetNames(type).Select(_ => new OpenApiString(_)));
            schema.Extensions.Add("x-enumNames", enumNames);

            if (EnumConsts.BaseEnumMaps.ContainsKey(type.Name))
            {
                return;
            }

            EnumConsts.BaseEnumMaps.Add(type.Name, new List<EnumMap>());


            var enumName = type.GetEnumNames();
            var enumFields = type.GetFields().ToList();

            if (enumName != null && enumName.Length > 0)
            {
                //var index = 0;
                foreach (var name in enumName)
                {
                    var enumItem = enumFields.Find(o => o.Name == name);
                    var attribute = enumItem.GetCustomAttribute(_displayNameAttributeType);
                    if (attribute != null)
                    {
                        var displayNameAttribute = attribute as DescriptionAttribute;
                        if (displayNameAttribute != null)
                        {
                            EnumConsts.BaseEnumMaps[type.Name].Add(new EnumMap
                            {
                                Key = Convert.ToInt32(enumItem.GetValue(type)),
                                Value = enumItem.Name,
                                DisplayName = displayNameAttribute.Description
                            });
                        }

                    }
                }
            }
        }
    }
}
