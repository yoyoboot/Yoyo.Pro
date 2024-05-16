using System.ComponentModel.DataAnnotations;

namespace Yoyo.LowCode.DbLinks.Dtos
{
    public class CreateOrUpdateDbLinkInput
    {
        [Required]
        public DbLinkEditDto DbLink { get; set; }

        //// custom codes

        //// custom codes end
    }
}
