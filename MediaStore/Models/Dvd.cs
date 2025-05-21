namespace MediaStore.Models
{
    public class Dvd : Media
    {
        public string DvdType { get; set; } = null!;

        public string Director { get; set; } = null!;

        public int Runtime { get; set; }

        public string Studio { get; set; } = null!;

        public string Language { get; set; } = null!;

        public string Subtitles { get; set; } = null!;

        public DateOnly? ReleasedDate { get; set; }

        public string? Genre { get; set; }
    }
}