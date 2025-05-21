using System;
using System.Collections.Generic;

namespace MediaStore.Data
{
    public partial class PaymentTransaction
    {
        public int TransactionId { get; set; }

        public DateTime PaymentTime { get; set; }

        public int PaymentAmount { get; set; }

        public string Content { get; set; } = null!;

        public string BankTransactionId { get; set; } = null!;

        public string CardType { get; set; } = null!;

        public virtual ICollection<Invoice> Invoices { get; set; } = new List<Invoice>();
    }
}
