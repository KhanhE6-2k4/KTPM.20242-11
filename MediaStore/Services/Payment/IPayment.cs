using MediaStore.ViewModels;

namespace MediaStore.Services.Payment
{
    public interface IPayment
    {
        Task<PaymentResult> PayOrder(InvoiceViewModel invoice);

        Task<RefundResult> Refund(RefundRequest request);

    }
}