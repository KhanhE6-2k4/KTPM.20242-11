using MediaStore.Data;

namespace MediaStore.ViewModels
{
    public class ProductViewModel
    {
        public int MediaId { get; set; }
        public string Title { get; set; } = null!;

        public string? ImageUrl { get; set; }
        public int Price { get; set; }

        public string Description { get; set; } = null!;

        public string Type { get; set; } = null!;
        // public string Category { get; set; } = null!;

        public int TotalQuantity { get; set; }

    }
    public class BookViewModel : ProductViewModel
    {
        public string Authors { get; set; } = null!;

        public string CoverType { get; set; } = null!;

        public string Publisher { get; set; } = null!;

        public DateOnly PublicationDate { get; set; }

        public int? Pages { get; set; }

        public string? Language { get; set; }
        public string? Genre { get; set; }

        public BookViewModel(Book book)
        {
            MediaId = book.MediaId;
            Title = book.Title;
            ImageUrl = book.ImageUrl;
            Price = book.Price;
            Description = book.Description;
            Type = "Book";
            TotalQuantity = book.TotalQuantity;
            Authors = book.Authors;
            CoverType = book.CoverType;
            Publisher = book.Publisher;
            PublicationDate = book.PublicationDate;
            Pages = book.Pages;
            Language = book.Language;
            Genre = book.Genre;
        }

    }
    public class DvdViewModel : ProductViewModel
    {
        public string DvdType { get; set; } = null!;

        public string Director { get; set; } = null!;

        public int Runtime { get; set; }

        public string Studio { get; set; } = null!;

        public string Language { get; set; } = null!;

        public string Subtitles { get; set; } = null!;

        public DateOnly? ReleasedDate { get; set; }

        public string? Genre { get; set; }

        public DvdViewModel(Dvd dvd)
        {
            MediaId = dvd.MediaId;
            Title = dvd.Title;
            ImageUrl = dvd.ImageUrl;
            Price = dvd.Price;
            Description = dvd.Description;
            Type = "DVD";
            TotalQuantity = dvd.TotalQuantity;
            DvdType = dvd.DvdType;
            Director = dvd.Director;
            Runtime = dvd.Runtime;
            Studio = dvd.Studio;
            Language = dvd.Language;
            Subtitles = dvd.Subtitles;
            ReleasedDate = dvd.ReleasedDate;
            Genre = dvd.Genre;
        }

    }
    public class CdViewModel : ProductViewModel
    {
        public string Artists { get; set; } = null!;

        public string RecordLabel { get; set; } = null!;

        public string TrackList { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public DateOnly? ReleaseDate { get; set; }

        public CdViewModel(CdAndLp cd)
        {
            MediaId = cd.MediaId;
            Title = cd.Title;
            ImageUrl = cd.ImageUrl;
            Price = cd.Price;
            Description = cd.Description;
            Type = "CD";
            TotalQuantity = cd.TotalQuantity;
            Artists = cd.Artists;
            RecordLabel = cd.RecordLabel;
            TrackList = cd.TrackList;
            Genre = cd.Genre;
            ReleaseDate = cd.ReleaseDate;
        }
    }

    public class LpViewModel : ProductViewModel
    {
        public string Artists { get; set; } = null!;

        public string RecordLabel { get; set; } = null!;

        public string TrackList { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public DateOnly? ReleaseDate { get; set; }

        public LpViewModel(CdAndLp lp)
        {
            MediaId = lp.MediaId;
            Title = lp.Title;
            ImageUrl = lp.ImageUrl;
            Price = lp.Price;
            Description = lp.Description;
            Type = "LP";
            TotalQuantity = lp.TotalQuantity;
            Artists = lp.Artists;
            RecordLabel = lp.RecordLabel;
            TrackList = lp.TrackList;
            Genre = lp.Genre;
            ReleaseDate = lp.ReleaseDate;
        }

    }
}