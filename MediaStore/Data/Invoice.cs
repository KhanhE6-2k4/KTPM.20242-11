using System;
using System.Collections.Generic;

namespace MediaStore.Data
{
    public partial class Invoice
    {
        public int InvoiceId { get; set; }

        public int TotalAmount { get; set; }

        public int TransactionId { get; set; }

        public int OrderId { get; set; }

        public virtual OrderInfo Order { get; set; } = null!;

        public virtual PaymentTransaction Transaction { get; set; } = null!;
    }
}
