using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.ViewModelUser
{
    public  class MenuVm
    {

        public int id { get; set; }
        [Required]
        [StringLength(50)]
        public string text { get; set; }
        public int? MainMenu { get; set; }
        public int? Section { get; set; }
        public bool? Show { get; set; }
        public int? ApplicationId { get; set; }
        [StringLength(150)]
        public string Icon { get; set; }
        [StringLength(150)]
        public string HeaderName { get; set; }
        public bool? IsFavorite { get; set; }
        public int? MenuOrder { get; set; }
        [Required]
        [StringLength(150)]
        public string MenuUrl { get; set; }
        [StringLength(150)]
        public string MenuText { get; set; }
        public bool? IsMain { get; set; }

        public bool IsGroup { get; set; }
        public bool selected { get; set; }
        public List<MenuVm> children { get; set; }
    }
}
