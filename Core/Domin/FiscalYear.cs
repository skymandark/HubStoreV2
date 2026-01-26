using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public class FiscalYear
    {
        [Key]
        public int FiscalYearId { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime StartDate { get; set; }

        [Required]
        [Column(TypeName = "date")]
        public DateTime EndDate { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public bool IsClosed { get; set; } = false;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [MaxLength(100)]
        public string CreatedBy { get; set; }

        public virtual ICollection<OpeningBalance> OpeningBalances { get; set; }
        public string YearName { get; set; }
    }
}
