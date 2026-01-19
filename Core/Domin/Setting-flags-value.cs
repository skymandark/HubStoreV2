using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class Setting_flags_item
    {
        [Display(Name = "اسم الخاصية")]
        public int FlagDetailId { get; set; }

        [ForeignKey("FlagDetailId")]
        [Display(Name = "اسم الخاصية")]
        public Setting_flag_details Setting_flag_details { get; set; }

        [StringLength(250, ErrorMessage = "لا يمكن أن تتعدي 250 حرف")]
        [Display(Name = "القيمة")]
        public string FlagValue { get; set; }
    }
}
