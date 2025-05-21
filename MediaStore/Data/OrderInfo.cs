using System;
using System.Collections.Generic;

namespace MediaStore.Data
{
    public partial class OrderInfo
    {
        public int OrderId { get; set; }

        public int ShippingFees { get; set; }

        public int Subtotal { get; set; }

        public string Status { get; set; } = null!;

        public int DeliveryId { get; set; }

        public virtual DeliveryInfo Delivery { get; set; } = null!;

        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();

        public virtual ICollection<OrderMedia> OrderMedia { get; set; } = new List<OrderMedia>();

        public virtual ICollection<RushOrderInfo> RushOrderInfos { get; set; } = new List<RushOrderInfo>();
    }
}
