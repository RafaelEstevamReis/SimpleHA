using Newtonsoft.Json;

namespace Simple.HAApi.Models
{
    public class ContextModel
    {
        public string Id { get; set; }
        [JsonProperty("parent_id")]
        public string ParentId { get; set; }
        [JsonProperty("user_id")]
        public string UserId { get; set; }
    }
}
