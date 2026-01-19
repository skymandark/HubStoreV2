using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Domin
{
    public  class Receipt
    {
        public int ReceiptId { get; set; }
        public int OrderLineId { get; set; }
        public int MovementId { get; set; }
        public decimal QuantityReceived { get; set; }
        public decimal QuantityBaseReceived { get; set; }
        public DateTime ReceivedAt { get; set; }
        public string CreatedBy { get; set; }

        [ForeignKey(nameof(OrderLineId))]
        public virtual OrderLine OrderLine { get; set; }

        [ForeignKey(nameof(MovementId))]
        public virtual Movement Movement { get; set; }
    }
}
