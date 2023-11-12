namespace Yoyo.LowCode.Models
{
    /// <summary>
    ///
    /// </summary>
    public class CodeGeneratorFuncSwitchModel
    {
        /// <summary>
        /// 功能开关名称
        /// </summary>
        public string funcSwitch { get; set; }

        /// <summary>
        /// 表关系标记
        /// 0-主表,1-子表
        /// </summary>
        public int tableRelationTag { get; set; }

        /// <summary>
        /// 关联主键
        /// </summary>
        public string relationField { get; set; }
    }
}
