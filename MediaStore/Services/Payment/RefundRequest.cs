namespace MediaStore.Services.Payment
{
    public class RefundRequest
    {
        public string TransactionId { get; set; }
        public int amount { get; set; }
        public string? Reason { get; set; }
    }
}