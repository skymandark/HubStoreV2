using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class FlagType
    {
        [Key]
        public int FlagTypeId { get; set; }

        [Required(ErrorMessage = "برجاء ادخال طبيعة الخاصية")]
        [Display(Name = "طبيعة الخاصية")]
        public string FlagTypeName { get; set; }
    }
}
