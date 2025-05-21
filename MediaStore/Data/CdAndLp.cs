using System;
using System.Collections.Generic;

namespace MediaStore.Data
{
    public partial class CdAndLp : Media
    {
        // public int MediaId { get; set; }

        public bool IsCd { get; set; }

        public string Artists { get; set; } = null!;

        public string RecordLabel { get; set; } = null!;

        public string TrackList { get; set; } = null!;

        public string Genre { get; set; } = null!;

        public DateOnly? ReleaseDate { get; set; }

        public virtual Media Media { get; set; } = null!;
    }
}
