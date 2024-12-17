using System.Text.Json.Serialization;

namespace Entities.DTOs
{
    /// <summary>
    /// OpenAI request DTO
    /// </summary>
    public class OpenAIRequestDto
    {
        [JsonPropertyName("model")]
        public string? Model { get; set; }

        [JsonPropertyName("messages")]
        public List<OpenAIMessageRequestDto>? Messages { get; set; }

        [JsonPropertyName("temperature")]
        public float? Temperature { get; set; }

        [JsonPropertyName("max_tokens")]
        public int? MaxTokens { get; set; }
    }

    /// <summary>
    /// OPenAI message request DTO
    /// </summary>
    public class OpenAIMessageRequestDto
    {
        [JsonPropertyName("role")]
        public string? Role { get; set; }

        [JsonPropertyName("content")]
        public string? Content { get; set; }
    }
}
