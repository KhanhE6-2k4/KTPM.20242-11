using System;
using System.Collections.Generic;

namespace MediaStore.Data
{
    public partial class Media
    {
        public int MediaId { get; set; }

        public string Title { get; set; } = null!;

        public int Price { get; set; }

        public int TotalQuantity { get; set; }

        public double Weight { get; set; }

        public bool? RushOrderSupported { get; set; }

        public string? ImageUrl { get; set; }

        public string Barcode { get; set; } = null!;

        public string Description { get; set; } = null!;

        public string? ProductDimension { get; set; }

        public DateOnly? ImportDate { get; set; }

        public virtual Book? Book { get; set; }

        public virtual CdAndLp? CdAndLp { get; set; }

        public virtual Dvd? Dvd { get; set; }

        public virtual ICollection<OrderMedia> OrderMedia { get; set; } = new List<OrderMedia>();
    }
}
