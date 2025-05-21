namespace MediaStore.Services.Payment
{
    public class RefundResult
    {
        public bool success { get; set; }
        public string? Message { get; set; }
        public string? RefundTransactionId { get; set; }

    }
}