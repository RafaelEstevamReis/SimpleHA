namespace Simple.HAApi.Models;

using Newtonsoft.Json;

public class ConfigurationCheckModel
{
    [JsonProperty("errors")]
    public string Errors { get; set; }
    [JsonProperty("result")]
    public string Result { get; set; }
}
