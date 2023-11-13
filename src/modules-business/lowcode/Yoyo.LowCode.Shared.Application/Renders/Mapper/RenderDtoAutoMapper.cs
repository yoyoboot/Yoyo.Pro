// ReSharper disable once CheckNamespace
using AutoMapper;
using Yoyo.LowCode.Dtos;
using Yoyo.Pro.Modules.FileManager;

namespace Yoyo.LowCode.CustomDtoAutoMapper
{
    /// <summary>
    /// 配置BaseDbLink的AutoMapper映射
    /// 前往 <see cref="LowCodeApplicationModule"/>的AbpAutoMapper配置方法下添加以下代码段
    /// DbLinkDtoAutoMapper.CreateMappings(configuration);
    /// </summary>
    public static class RenderDtoAutoMapper
    {
        public static void CreateMappings(IMapperConfigurationExpression configuration)
        {
            configuration.CreateMap<LowCodeSysFileListDto, IBasicFile>().ReverseMap();

            //// custom codes

            //// custom codes end
        }
    }
}
