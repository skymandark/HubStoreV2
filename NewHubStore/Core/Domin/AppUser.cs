using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace Core.Domin
{
    public class AppUser:IdentityUser
    {
        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [MaxLength(50)]
        public string Phone { get; set; }

        [Required]
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedAt { get; set; }

        public DateTime? LastLoginDate { get; set; }

        public virtual ICollection<Order> OrdersRequested { get; set; }
        public virtual ICollection<ApprovalHistory> ApprovalHistories { get; set; }
        public virtual ICollection<AuditTrail> AuditTrails { get; set; }
        public virtual ICollection<AttemptLog> AttemptLogs { get; set; }
        public virtual ICollection<ExportLog> ExportLogs { get; set; }
        public virtual ICollection<SavedView> SavedViews { get; set; }
        public virtual ICollection<Notification> Notifications { get; set; }
        public virtual ICollection<ItemUnitHistory> ItemUnitHistories { get; set; }
    }
}
