using System;
using System.Collections.Generic;

namespace MediaStore.Data
{
    public partial class Dvd : Media
    {
        // public int MediaId { get; set; }

        public string DvdType { get; set; } = null!;

        public string Director { get; set; } = null!;

        public int Runtime { get; set; }

        public string Studio { get; set; } = null!;

        public string Language { get; set; } = null!;

        public string Subtitles { get; set; } = null!;

        public DateOnly? ReleasedDate { get; set; }

        public string? Genre { get; set; }

        public virtual Media Media { get; set; } = null!;
    }
}
