using RestWithASPNet.Hypermedia;
using RestWithASPNet.Hypermedia.Abstract;
using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace RestWithASPNet.Data.VO
{
    public class PersonVO : ISupportHyperMedia
    {
        [JsonPropertyName("cod")]
        public long Id { get; set; }

        [JsonPropertyName("name")]
        public string FirstName { get; set; }

        [JsonPropertyName("last_name")]
        public string SecondNamed { get; set; }

        [JsonPropertyName("country")]
        public string Address { get; set; }

        //[JsonIgnore]
        public string Gender { get; set; }

        [JsonPropertyName("active")]
        public bool Enabled { get; set; }

        public List<HyperMediaLink> Links { get; set; } = new List<HyperMediaLink>();
    }
}
