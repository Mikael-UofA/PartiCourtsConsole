using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace PrepareGeojson.Formats
{
    internal class Feature
    {
        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("properties")]
        public Dictionary<string, object> Properties { get; set; }

        [JsonProperty("geometry")]
        public JObject Geometry { get; set; }

        public Feature(string type, Dictionary<string, object> properties, JObject geometry)
        {
            Type = type;
            Properties = properties;
            Geometry = geometry;
        }
    }
}
