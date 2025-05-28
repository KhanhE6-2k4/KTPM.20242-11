namespace MediaStore.ViewModels
{
    public class VnPaymentResponseModel
    {
        public bool Success { get; set; }

        public int Amount { get; set; }

        public string BankCode { get; set; }

        public string CardType { get; set; }

        public string TransactionStatus { get; set; }
        public string PayDate { get; set; }
        public string PaymentMethod { get; set; }
        public string OrderDescription { get; set; }
        public string OrderId { get; set; }
        public string TransactionId { get; set; }
        public string Token { get; set; }
        public string VnPayResponseCode { get; set; }
    }
}