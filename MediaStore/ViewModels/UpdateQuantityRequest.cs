using System.Text.Json.Serialization;

namespace MediaStore.ViewModels
{
    public class UpdateQuantityRequest
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("action")]
        public string? Action { get; set; }
        [JsonPropertyName("qty")]
        public int Qty { get; set; }
    }
}