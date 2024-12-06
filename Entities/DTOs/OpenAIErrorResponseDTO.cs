using System.Text.Json.Serialization;

namespace Entities.DTOs
{
    /// <summary>
    /// OpenAI error response DTO
    /// </summary>
    public class OpenAIErrorResponseDto
    {
        [JsonPropertyName("error")]
        public OpenAIError? Error { get; set; }
    }

    /// <summary>
    /// OpenAI error
    /// </summary>
    public class OpenAIError
    {
        [JsonPropertyName("message")]
        public string? Message { get; set; }

        [JsonPropertyName("type")]
        public string? Type { get; set; }

        [JsonPropertyName("param")]
        public string? Param { get; set; }

        [JsonPropertyName("code")]
        public string? Code { get; set; }
    }
}
