using System.Text.Json.Serialization;

namespace MediaStore.ViewModels
{
    public class OrderStatusUpdateModel
    {
        [JsonPropertyName("orderId")]
        public int OrderId { get; set; }

        [JsonPropertyName("newStatus")]
        public string NewStatus { get; set; }

    }
}