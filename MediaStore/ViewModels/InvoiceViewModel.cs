namespace MediaStore.ViewModels
{
    public class InvoiceViewModel
    {
        public string? name { get; set; }

        public string? phone { get; set; }

        public string? address { get; set; }

        public string? email { get; set; }

        public bool hasRushOrder { get; set; }
        public List<CartItem> regularItem { get; set; }

        public int regularSubTotal { get; set; }

        public int regularVAT => (int)(regularSubTotal * 0.10m);

        public int regularShippingFee { get; set; }
        public List<CartItem> rushItem { get; set; }

        public int rushSubTotal { get; set; }

        public int rushVAT => (int)(rushSubTotal * 0.10m);
        public int rushShippingFee { get; set; }

        public int TotalPrice => regularSubTotal + regularVAT + regularShippingFee + rushSubTotal + rushVAT + rushShippingFee;
    }
}