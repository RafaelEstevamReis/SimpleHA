using Newtonsoft.Json;

namespace Simple.HAApi.Models
{
    public class ConfigurationReloadModel
    {
        [JsonProperty("require_restart")]
        public bool RequireRestart { get; set; }
    }
}
