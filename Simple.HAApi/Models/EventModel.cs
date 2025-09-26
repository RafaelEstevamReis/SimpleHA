namespace Simple.HAApi.Models;

using Newtonsoft.Json;

public class EventModel
{
    [JsonProperty("event_type")]
    public string EventType { get; set; }
    [JsonProperty("data")]
    public EventData EventData { get; set; }

    public override string ToString()
    {
        return $"[{EventType}] {EventData?.EntityId}";
    }
}
public class EventData
{
    [JsonProperty("entity_id")]
    public string EntityId { get; set; }


    [JsonProperty("old_state")]
    public StateModel OldState { get; set; }
    [JsonProperty("new_state")]
    public StateModel NewState { get; set; }

}
