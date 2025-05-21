using Bookstore.Models;
using MediaStore.ViewModels;

namespace MediaStore.Services
{
    public class ShippingService : IShippingService
    {
        public int CalculateRegularFee(DeliveryForm form, List<CartItem> cart)
        {
            return 10;
        }

        public int CalculateRushFee(DeliveryForm form, List<CartItem> cart)
        {
            return 30; // cố định hoặc tính theo khối lượng hàng, địa chỉ,...
        }
    }

}