using System;
using System.Collections.Generic;

namespace MediaStore.Data
{
    public partial class DeliveryInfo
    {
        public int DeliveryId { get; set; }

        public string Name { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string Province { get; set; } = null!;

        public string Address { get; set; } = null!;

        public string? Message { get; set; }

        public virtual ICollection<OrderInfo> OrderInfos { get; set; } = new List<OrderInfo>();
    }
}
