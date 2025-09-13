using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Globalization;

namespace Simple.HAApi.Models
{
    public class StateModel
    {
        [JsonProperty("entity_id")]
        public string EntityId { get; set; }
        public string State { get; set; }
        public Dictionary<string, object> Attributes { get; set; }
        public ContextModel Context { get; set; }
        [JsonProperty("last_changed")]
        public DateTimeOffset LastChanged { get; set; }
        [JsonProperty("last_updated")]
        public DateTimeOffset LastUpdated { get; set; }

        public T GetAttribute<T>(string name)
        {
            if (!Attributes.ContainsKey(name)) return default;
            // Try internal conversion
            if (typeof(JToken).IsAssignableFrom(Attributes[name].GetType()))
            {
                return ((JToken)Attributes[name]).ToObject<T>();
            }
            // Direct cast
            return (T)Attributes[name];
        }

        public TimeSpan ChangeAge
            => DateTime.UtcNow - LastChanged;
        public TimeSpan UpdateAge
            => DateTime.UtcNow - LastChanged;
        public string FriendlyName
            => GetAttribute<string>("friendly_name");

        public string Domain => EntityId?.Split('.')[0];

        decimal? dval;
        public bool GetDecimalState(out decimal dState)
        {
            if (dval != null)
            {
                dState = dval.Value;
                return true;
            }

            var result = decimal.TryParse(State, NumberStyles.Number, CultureInfo.InvariantCulture, out dState);
            dval = dState;
            return result;
        }
        public bool GetDateTimeState(out DateTime dtState)
        {
            return DateTime.TryParse(State, CultureInfo.InvariantCulture, DateTimeStyles.None, out dtState);
        }
        public bool IsUnavailable => "Unavailable".Equals(State, StringComparison.InvariantCultureIgnoreCase);
        public bool IsUnkown => "Unknown".Equals(State, StringComparison.InvariantCultureIgnoreCase);

        public override string ToString() => $"{FriendlyName ?? EntityId}: {State}";
    }
}