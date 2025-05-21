namespace MediaStore.ViewModels
{
    public class CartItem
    {
        public int Id { get; set; }
        public string? ImageUrl { get; set; }
        public string Title { get; set; } = null!;
        public int Price { get; set; }

        public int Qty { get; set; }

        public double Weight { get; set; }

        public int TotalQuantity { get; set; }

        public bool IsRushOrderSupported { get; set; }

        public int Amount => Qty * Price;

    }
}