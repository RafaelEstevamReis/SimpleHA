using Newtonsoft.Json;

namespace Simple.HAApi.Models
{
    public class ConfigurationCheckModel
    {
        [JsonProperty("errors")]
        public string Errors { get; set; }
        [JsonProperty("result")]
        public string Result { get; set; }
    }
}
