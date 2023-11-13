using System.ComponentModel.DataAnnotations;

namespace Yoyo.LowCode.DynamicMenus.Dtos
{
    public class CreateOrUpdateDynamicMenuInput
    {
        [Required]
        public LowCodeDynamicMenuEditDto DynamicMenu { get; set; }

        //// custom codes

        //// custom codes end
    }
}
