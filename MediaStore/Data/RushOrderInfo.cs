using System;
using System.Collections.Generic;

namespace MediaStore.Data
{
    public partial class RushOrderInfo
    {
        public int RushId { get; set; }

        public DateTime? DeliveryTime { get; set; }

        public string Instruction { get; set; } = null!;

        public int OrderId { get; set; }

        public virtual OrderInfo Order { get; set; } = null!;
    }
}
