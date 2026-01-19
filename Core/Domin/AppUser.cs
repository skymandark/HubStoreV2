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
        public int? UserSerial { get; set; }

        public int? EmployeeId { get; set; }

        [StringLength(50)]
        public string PinCode { get; set; }

        public bool IsAdmin { get; set; }

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
