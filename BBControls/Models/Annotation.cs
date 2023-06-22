using System.Text.Json.Serialization;

namespace BBControls.Models
{

    public class Annotation
    {
        [JsonPropertyName("id")]
        public int Id { get; set; }

        [JsonPropertyName("beginTime")]
        public int BeginTime { get; set; }

        [JsonPropertyName("description")]
        public string? Description { get; set; }

        [JsonPropertyName("endTime")]
        public int? EndTime { get; set; }

        [JsonPropertyName("hashTags")]
        public string? HashTags { get; set; }

        [JsonPropertyName("imdbId")]
        public string? ImdbId { get; set; }

        [JsonPropertyName("likes")]
        public int? Likes { get; set; }

        [JsonPropertyName("title")]
        public string? Title { get; set; }

        [JsonPropertyName("user")]
        public string? User { get; set; }
    }


}
