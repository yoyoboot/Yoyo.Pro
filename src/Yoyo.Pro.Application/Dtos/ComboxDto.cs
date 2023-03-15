namespace Yoyo.Pro.Dtos
{
    /// <summary>
    /// Combox公用Dto
    /// </summary>
    /// <typeparam name="TVal"></typeparam>
    public class ComboxDto<TVal>
    {
        /// <summary>
        /// 键-显示名称
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// 值-实际值
        /// </summary>
        public TVal Val { get; set; }

        public ComboxDto()
        {

        }

        public ComboxDto(string label, TVal val)
        {
            Label = label;
            Val = val;
        }
    }
}
