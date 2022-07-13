using Newtonsoft.Json;

namespace Simple.HAApi.Models
{
    public class EventListenersModel
    {
        public string Event { get; set; }
        [JsonProperty("listener_count")]
        public int ListenerCount { get; set; }
    }
}
