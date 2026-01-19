using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class Setting_flag_master
    {
        [Key]
        public int FlagMasterId { get; set; }

        [StringLength(250, ErrorMessage = "لا يمكن أن تتعدي 250 حرف")]
        [Display(Name = "اسم الخاصية")]
        [Required(ErrorMessage = "برجاء ادخال اسم الخاصية")]
        public string FlagMasterName { get; set; }

        [Display(Name = "عدد القيم")]
        public int? FlagCount { get; set; }

        [Display(Name = "يجب اختيار قيمة واحدة فقط")]
        public bool? OneSelection { get; set; }
    }
}
