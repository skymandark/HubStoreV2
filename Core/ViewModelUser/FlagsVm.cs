using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Domin;
using System.Web.Mvc;
//using Core.Domin.Approval;

namespace Core.ViewModelUser
{
    public  class FlagsVm
    {

        public int FlagId { get; set; }
        public string FlagName { get; set; }
        public string ApplicationName { get; set; }
        public string MenuName { get; set; }
        public string UserId { get; set; }
        public string FlagValue { get; set; }
        public string Description { get; set; }
        public bool IsExist { get; set; }
    }

    public class ItemFlagsVm
    {
        public int BaseId { get; set; }

        [Display(Name = "نوع الخاصية")]
        public int? FlagTypeId { get; set; }

        [Display(Name = "نوع الخاصية")]
        public string FlagTypeName { get; set; }

        [Display(Name = "كود الخاصية")]
        public int ItemFlagMasterId { get; set; }

        [Display(Name = "اسم الخاصية")]

        public string ItemFlagMasterName { get; set; }

        [Display(Name = "اسم الخاصية")]
        //public ItemFlagMaster ItemFlagMaster { get; set; }


        [StringLength(250, ErrorMessage = "لا يمكن أن تتعدي 250 حرف")]
        //[Display(Name = "اسم الخاصية الفرعية")]
        [Required(ErrorMessage = "برجاء ادخال اسم الخاصية الفرعية")]
        public string FlagDetailName { get; set; }


        [Display(Name = "يتطلب ادخالها؟")]
        public bool? IsRequired { get; set; }

        public string Notes { get; set; }


        [Display(Name = "اسم الخاصية")]
        public string FlagMasterName { get; set; }


        [Required(ErrorMessage = "برجاء اختيار طبيعة الخاصية")]
        [Display(Name = "طبيعة الخاصية")]
        public int FlagNatureId { get; set; }


        [Display(Name = "طبيعة الخاصية")]
        public string FlagNatureName { get; set; }



        [Display(Name = "عدد القيم")]
        public int? FlagCount { get; set; }


        public int? Count { get; set; }


        //public IEnumerable<ItemFlagMaster> ItemFlagMasters { get; set; }    
        //public IList<ItemFlagDetail> ItemFlagDetailList { get; set; }
        //public IList<ItemFlagValue> ItemFlagValueList { get; set; }


        //public IEnumerable<FlagNature> FlagNatureList { get; set; }
        public IEnumerable<FlagType> FlagTypeList { get; set; }


        //public ItemFlagMaster FlagMaster { get; set; }
        //public ItemFlagDetail ItemFlagDetail { get; set; }
        //public ItemFlagValue ItemFlagValue { get; set; }

        //public FlagNature FlagNature { get; set; }
        public FlagType FlagType { get; set; }



        public int ItemId { get; set; }

        [Display(Name = "الخواص")]
        public List<int> ItemFlagDetailId { get; set; }
        public List<int> SelectedValues { get; set; }
        public SelectList AllValues { get; set; }

        public SelectList MasterFlags { get; set; }
        public int FlagMasterId { get; set; }

    }


    public class AddItemFlagMaster
    {
        public int BaseId { get; set; }

        [Display(Name = "الخواص")]
        public List<int> ItemFlagDetailId { get; set; }
        public List<int> SelectedValues { get; set; }
        public SelectList AllValues { get; set; }

        public SelectList MasterFlags { get; set; }
        public int FlagMasterId { get; set; }

        //public IList<ItemFlagDetail> ItemFlagDetailList { get; set; }
        //public IList<ItemFlagValue> ItemFlagValueList { get; set; }

    }

}
