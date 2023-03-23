using System;
using System.Collections.Generic;

namespace Simple.HAApi.Models
{
    public class HistoryModel
    {
        public EntityStateChangeModel[][] Items { get; set; }
    }
    public class EntityStateChangeModel
    {
        public string entity_id { get; set; }
        public object state { get; set; }
        public Dictionary<string, string> attributes { get; set; }
        public DateTime last_changed { get; set; }
        public DateTime last_updated { get; set; }
    }
}
