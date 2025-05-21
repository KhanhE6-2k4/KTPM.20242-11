using MediaStore.ViewModels;

namespace MediaStore.Services
{
    public interface IShippingService
    {
        int CalculateRegularFee(DeliveryForm form, List<CartItem> cart);
        int CalculateRushFee(DeliveryForm form, List<CartItem> cart);
    }
}