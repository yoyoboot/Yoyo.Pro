// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using AutoMapper;
using Yoyo.LowCode.ComField.Dtos;
using Yoyo.LowCode.Databases.Entity;

namespace Yoyo.LowCode.ComField.Mapper
{
    /// <summary>
    /// 配置BaseDbLink的AutoMapper映射
    /// 前往 <see cref="LowCodeApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    /// DbLinkDtoAutoMapper.CreateMappings(configuration);
    /// </summary>
    public class ComFieldDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<BaseComFields, ComFieldListDto>();
            configuration.CreateMap<ComFieldListDto, BaseComFields>();

            configuration.CreateMap<ComFieldEditDto, BaseComFields>();
            configuration.CreateMap<BaseComFields, ComFieldEditDto>();

            configuration.CreateMap<BaseComFields, ComFieldSelectorOutput>().ReverseMap();
            //// custom codes

            //// custom codes end
        }
    }
}
