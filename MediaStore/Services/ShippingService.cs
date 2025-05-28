using Bookstore.Models;
using MediaStore.ViewModels;

namespace MediaStore.Services
{
    public class ShippingService : IShippingService
    {
        public int CalculateRegularFee(DeliveryForm form, List<CartItem> cart)
        {
            return 15000;
        }

        public int CalculateRushFee(DeliveryForm form, List<CartItem> cart)
        {
            return 30000; // cố định hoặc tính theo khối lượng hàng, địa chỉ,...
        }
    }

}