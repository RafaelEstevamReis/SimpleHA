namespace Simple.HAApi.Models;

using Newtonsoft.Json;

public class EventListenersModel
{
    public string Event { get; set; }
    [JsonProperty("listener_count")]
    public int ListenerCount { get; set; }
}
