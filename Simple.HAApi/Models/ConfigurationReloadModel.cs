namespace Simple.HAApi.Models;

using Newtonsoft.Json;

public class ConfigurationReloadModel
{
    [JsonProperty("require_restart")]
    public bool RequireRestart { get; set; }
}
