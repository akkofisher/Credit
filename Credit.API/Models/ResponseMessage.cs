using System.Text.Json.Serialization;

namespace Credit.API.Models
{
    public sealed class ResponseMessage<T>
    {
        [JsonPropertyName("success")] 
        public bool Success { get; init; }

        [JsonPropertyName("data")] 
        public T? Data { get; init; }
    }
}
