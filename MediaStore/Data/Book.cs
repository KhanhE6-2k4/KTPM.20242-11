using System;
using System.Collections.Generic;

namespace MediaStore.Data
{
    public partial class Book : Media
    {
        // public int MediaId { get; set; }

        public string Authors { get; set; } = null!;

        public string CoverType { get; set; } = null!;

        public string Publisher { get; set; } = null!;

        public DateOnly PublicationDate { get; set; }

        public int? Pages { get; set; }

        public string? Language { get; set; }

        public string? Genre { get; set; }

        public virtual Media Media { get; set; } = null!;
    }
}
