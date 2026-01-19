using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;      // للـ Display, StringLength, Required
using System.ComponentModel.DataAnnotations.Schema; // للـ ForeignKey

namespace Core.Domin
{
    public class Setting_flag_details
    {
        [Display(Name = "اسم الخاصية")]
        public int FlagMasterId { get; set; }

        [Display(Name = "اسم الخاصية")]
        [ForeignKey("FlagMasterId")]
        public Setting_flag_master Setting_flag_master { get; set; }

        [Display(Name = "نوع الخاصية")]
        public int? FlagTypeId { get; set; }

        [ForeignKey("FlagTypeId")]
        [Display(Name = "نوع الخاصية")]
        public FlagType FlagType { get; set; }


        [StringLength(250, ErrorMessage = "لا يمكن أن تتعدي 250 حرف")]
        [Display(Name = "اسم الخاصية الفرعية")]
        [Required(ErrorMessage = "برجاء ادخال اسم الخاصية الفرعية")]
        public string FlagDetailName { get; set; }

        [Display(Name = "يتطلب ادخالها؟")]
        public bool? IsRequired { get; set; }


        [StringLength(250, ErrorMessage = "لا يمكن أن تتعدي 250 حرف")]
        [Display(Name = "القيمة")]
        public string FlagValue { get; set; }
    }
}
