using System.Text.Json.Serialization;

namespace RestWithASPNet.Data.VO
{
    public class PersonVO
    {
        [JsonPropertyName("cod")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string SecondNamed { get; set; }

        [JsonPropertyName("country")]
        public string Address { get; set; }

        [JsonIgnore]
        public string Gender { get; set; }
    }
}
