using System;
using System.Collections.Generic;

namespace MediaStore.Data
{
    public partial class OrderMedia
    {
        public int MediaId { get; set; }

        public int OrderId { get; set; }

        public int Quantity { get; set; }

        public int OrderType { get; set; }

        public virtual Media Media { get; set; } = null!;

        public virtual OrderInfo Order { get; set; } = null!;
    }
}
