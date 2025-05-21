namespace MediaStore.Models
{
    public class Book : Media
    {
        public string Authors { get; set; } = null!;

        public string CoverType { get; set; } = null!;

        public string Publisher { get; set; } = null!;

        public DateOnly PublicationDate { get; set; }

        public int? Pages { get; set; }

        public string? Language { get; set; }

        public string? Genre { get; set; }
    }
}