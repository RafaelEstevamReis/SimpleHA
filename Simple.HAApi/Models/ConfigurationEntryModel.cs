namespace Simple.HAApi.Models;

using Newtonsoft.Json;

public class ConfigurationEntriesModel
{
    [JsonProperty("entry_id")]
    public string EntryId { get; set; }
    public string Domain { get; set; }
    public string Title { get; set; }
    public string Source { get; set; }
    public string State { get; set; }
    [JsonProperty("supports_options")]
    public bool SupportsOptions { get; set; }
    [JsonProperty("supports_remove_device")]
    public bool SupportsRemoveDevice { get; set; }
    [JsonProperty("supports_unload")]
    public bool SupportsUnload { get; set; }
    [JsonProperty("pref_disable_new_entities")]
    public bool PrefDisableNewEntities { get; set; }
    [JsonProperty("pref_disable_polling")]
    public bool PrefDisablePolling { get; set; }
    [JsonProperty("disabled_by")]
    public object DisabledBy { get; set; }
    public string Reason { get; set; }

    public override string ToString()
    {
        return $"[{State}] {Domain} {Title}";
    }

}
