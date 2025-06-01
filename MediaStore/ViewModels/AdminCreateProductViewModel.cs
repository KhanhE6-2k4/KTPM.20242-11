using System;
using System.ComponentModel.DataAnnotations;

namespace MediaStore.ViewModels
{
    public class AdminCreateProductViewModel
    {
        [Required(ErrorMessage = "Vui lòng chọn loại media.")]
        public string MediaType { get; set; } // "Book", "DVD", "CD", "LP"

        // Common Media Properties
        [Required(ErrorMessage = "Tiêu đề không được để trống.")]
        [StringLength(50, ErrorMessage = "Tiêu đề không được vượt quá 50 ký tự.")] // Sửa lại khớp với varchar(50) trong script.sql
        public string Title { get; set; } = null!;

        [Required(ErrorMessage = "Giá không được để trống.")]
        [Range(0, int.MaxValue, ErrorMessage = "Giá phải là số dương.")]
        public int Price { get; set; }

        [Required(ErrorMessage = "Số lượng không được để trống.")]
        [Range(0, int.MaxValue, ErrorMessage = "Số lượng phải là số dương hoặc bằng 0.")]
        public int TotalQuantity { get; set; }

        [Required(ErrorMessage = "Cân nặng không được để trống.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Cân nặng phải lớn hơn 0.")]
        public double Weight { get; set; }

        public bool RushOrderSupported { get; set; }

        [StringLength(200, ErrorMessage = "URL hình ảnh không được vượt quá 200 ký tự.")] // Khớp với varchar(200)
        [Display(Name = "URL Hình ảnh")]
        public string? ImageUrl { get; set; }

        [Required(ErrorMessage = "Barcode không được để trống.")]
        [StringLength(50, ErrorMessage = "Barcode không được vượt quá 50 ký tự.")] // Khớp với varchar(50)
        public string Barcode { get; set; } = null!;

        [Required(ErrorMessage = "Mô tả không được để trống.")]
        [StringLength(255, ErrorMessage = "Mô tả không được vượt quá 255 ký tự.")] // Khớp với varchar(255)
        public string Description { get; set; } = null!;

        [StringLength(50, ErrorMessage = "Kích thước sản phẩm không được vượt quá 50 ký tự.")] // Khớp với varchar(50)
        [Display(Name = "Kích thước Sản phẩm")]
        public string? ProductDimension { get; set; }

        [DataType(DataType.Date)]
        [Display(Name = "Ngày Nhập Hàng")]
        public DateOnly? ImportDate { get; set; }

        // Book Specific Properties
        // Các trường này sẽ được validate bởi ModelState.IsValid nếu MediaType là "Book"
        // và logic trong controller sẽ kiểm tra chúng.
        // Việc thêm [Required] ở đây sẽ khiến chúng bắt buộc ngay cả khi MediaType không phải là Book,
        // trừ khi bạn dùng custom validation attribute hoặc IValidatableObject.
        // Hiện tại, logic kiểm tra trong controller là đủ.
        [StringLength(50, ErrorMessage = "Tên tác giả không được vượt quá 50 ký tự.")] // Sửa lại khớp với varchar(50)
        public string? Authors { get; set; }

        [StringLength(50, ErrorMessage = "Loại bìa không được vượt quá 50 ký tự.")] // Khớp với varchar(50)
        public string? CoverType { get; set; }

        [StringLength(50, ErrorMessage = "Nhà xuất bản không được vượt quá 50 ký tự.")] // Sửa lại khớp với varchar(50)
        public string? Publisher { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? PublicationDate { get; set; } // Bắt buộc cho Book trong controller

        public int? Pages { get; set; }

        [StringLength(50, ErrorMessage = "Ngôn ngữ sách không được vượt quá 50 ký tự.")] // Khớp với varchar(50)
        public string? BookLanguage { get; set; }

        [StringLength(50, ErrorMessage = "Thể loại sách không được vượt quá 50 ký tự.")] // Khớp với varchar(50)
        public string? BookGenre { get; set; }

        // DVD Specific Properties
        [StringLength(50, ErrorMessage = "Loại DVD không được vượt quá 50 ký tự.")] // Khớp với varchar(50)
        public string? DvdType { get; set; }

        [StringLength(50, ErrorMessage = "Tên đạo diễn không được vượt quá 50 ký tự.")] // Sửa lại khớp với varchar(50)
        public string? Director { get; set; }

        public int? Runtime { get; set; } // Bắt buộc cho DVD trong controller

        [StringLength(50, ErrorMessage = "Hãng phim không được vượt quá 50 ký tự.")] // Sửa lại khớp với varchar(50)
        public string? Studio { get; set; }

        [StringLength(50, ErrorMessage = "Ngôn ngữ DVD không được vượt quá 50 ký tự.")] // Khớp với varchar(50)
        public string? DvdLanguage { get; set; }

        [StringLength(50, ErrorMessage = "Phụ đề không được vượt quá 50 ký tự.")] // Sửa lại khớp với varchar(50)
        public string? Subtitles { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? ReleasedDate { get; set; }

        [StringLength(50, ErrorMessage = "Thể loại DVD không được vượt quá 50 ký tự.")] // Khớp với varchar(50)
        public string? DvdGenre { get; set; }

        // CD/LP Specific Properties
        [StringLength(50, ErrorMessage = "Tên nghệ sĩ không được vượt quá 50 ký tự.")] // Sửa lại khớp với varchar(50)
        public string? Artists { get; set; }

        [StringLength(50, ErrorMessage = "Hãng đĩa không được vượt quá 50 ký tự.")] // Sửa lại khớp với varchar(50)
        public string? RecordLabel { get; set; }

        [StringLength(200, ErrorMessage = "Danh sách bài hát không được vượt quá 200 ký tự.")] // Khớp với varchar(200)
        public string? TrackList { get; set; }

        [StringLength(50, ErrorMessage = "Thể loại CD/LP không được vượt quá 50 ký tự.")] // Khớp với varchar(50)
        public string? CdLpGenre { get; set; }

        [DataType(DataType.Date)]
        public DateOnly? CdLpReleaseDate { get; set; }
    }
}