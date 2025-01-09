using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PrepareGeojson.Formats
{
    internal class GeoJson
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("crs")]
        public JObject Crs { get; set; }

        [JsonProperty("features")]
        public List<Feature> Features { get; set; }

        public GeoJson(string type, string name, JObject crs, List<Feature> features)
        {
            Type = type;
            Name = name;
            Crs = crs;
            Features = features;
        }
    }
}
