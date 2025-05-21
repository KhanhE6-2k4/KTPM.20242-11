using System.ComponentModel.DataAnnotations;

namespace MediaStore.ViewModels
{
    public class DeliveryForm
    {
        [Required(ErrorMessage = "Please enter your name.")]
        [StringLength(50, ErrorMessage = "Name cannot exceed 50 characters")]
        public string Name { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your phone number.")]
        [RegularExpression(@"^(03|05|07|08|09)\d{8}$", ErrorMessage = "Invalid phone number format.")]
        public string Phone { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your email.")]
        [RegularExpression(@"^[a-zA-Z0-9._%+-]+@gmail\.com$", ErrorMessage = "Only Gmail addresses are allowed.")]
        public string Email { get; set; } = null!;

        [Required(ErrorMessage = "Please select a province/city.")]
        public string Province { get; set; } = null!;

        [Required(ErrorMessage = "Please enter your address.")]
        public string Address { get; set; } = null!;

        [StringLength(200, ErrorMessage = "Message cannot exceed 200 characters.")]
        public string? Message { get; set; }

        public bool IsRushOrder { get; set; }
    }
}
