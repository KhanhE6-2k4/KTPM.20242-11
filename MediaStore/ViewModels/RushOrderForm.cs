using System.ComponentModel.DataAnnotations;

namespace MediaStore.ViewModels
{
    public class RushOrderForm
    {

        [Display(Name = "Delivery Time")]
        [DataType(DataType.DateTime)]
        [DisplayFormat(DataFormatString = "{0:yyyy-MM-ddTHH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? DeliveryTime { get; set; }

        [StringLength(200, ErrorMessage = "Message cannot exceed 200 characters.")]
        public string? Instruction { get; set; }
    }

}