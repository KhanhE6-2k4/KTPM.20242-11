namespace MediaStore.ViewModels
{
    public class Order
    {
        public List<CartItem> cart { get; set; }
        public DeliveryForm deliveryInfo { get; set; }

        public RushOrderForm rushOrderInfo { get; set; }

        public int regularShippingFee { get; set; }
        public int rushShippingFee { get; set; }
    }
}