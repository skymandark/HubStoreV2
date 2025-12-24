using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Collections.Generic;

namespace YourNamespace.Pages.Setting
{
    public class FlagsSettingTableUpdatedModel : PageModel
    {
        public List<PharmacyFlagDetail> PharmacyFlagDetailList { get; set; }

        public void OnGet()
        {
            // Initialize the data (this can come from a database or other source)
            PharmacyFlagDetailList = new List<PharmacyFlagDetail>
            {
                new PharmacyFlagDetail { FlagDetailName = "«”„ 1", FlagTypeId = 1, IsRequired = true, FlagValue = "ﬁÌ„… 1" },
                new PharmacyFlagDetail { FlagDetailName = "«”„ 2", FlagTypeId = 2, IsRequired = false, FlagValue = "ﬁÌ„… 2" }
            };
        }
    }

    public class PharmacyFlagDetail
    {
        public string FlagDetailName { get; set; }
        public int FlagTypeId { get; set; }
        public bool IsRequired { get; set; }
        public string FlagValue { get; set; }
    }
}