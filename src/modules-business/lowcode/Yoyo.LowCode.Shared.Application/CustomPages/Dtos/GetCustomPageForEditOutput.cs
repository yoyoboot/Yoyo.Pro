using System.Collections.Generic;
using Yoyo.LowCode.LowCodeViewModels.Dtos;

namespace Yoyo.LowCode.CustomPages.Dtos
{
    /// <summary>
    /// 读取可编辑功能设计的Dto
    /// </summary>
    public class GetCustomPageForEditOutput
    {
        public CustomPageEditDto CustomPage { get; set; }
        public List<KeyValuePair<string, string>> PageStatusEnumTypeEnum { get; set; }

        public List<LowCodeModelRelationEditDto> CodeModelRelationList { get; set; }
        //// custom codes

        //// custom codes end
    }
}
