using System;
using static Yoyo.LowCode.AutomaticTables.Dtos.CreateOrUpdateTableData;

namespace Yoyo.LowCode.CustomPages.Dtos
{
    public class CreateOrUpdateCustomPageInput
    {
        public Guid? BaseCustomPageId { get; set; }

        public CreateOrUpdateTableDto CreateOrUpdateTable { get; set; }

        public bool IsAuto { get; set; }

        //// custom codes

        //// custom codes end
    }
}
