namespace MediaStore.Services.Payment
{
    public class PaymentResult
    {
        public bool Success { get; set; }
        public string? ReidrectUrl { get; set; } // dung cho redirect nhu VnPay
        public string? HtmlForm { get; set; } // dung cho post form nhu Paypal
        public string? Message { get; set; }
    }
}