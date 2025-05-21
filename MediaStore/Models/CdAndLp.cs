namespace MediaStore.Models
{
    public class CdAndLp : Media
    {
        public bool IsCd { get; set; }

        public string Artists { get; set; } = null!;

        public string RecordLabel { get; set; } = null!;

        public string TrackList { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public DateOnly? ReleaseDate { get; set; }
    }
}