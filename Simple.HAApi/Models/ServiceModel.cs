using Newtonsoft.Json;
using System.Collections.Generic;

namespace Simple.HAApi.Models
{
    public class ServiceModel
    {
        public string Domain { get; set; }
        public Dictionary<string, ServiceInfo> Services { get; set; }

        public override string ToString()
        {
            return $"Services for {Domain}";
        }
    }

    public class ServiceInfo
    {
        public string Description { get; set; }
        public Dictionary<string, ServiceFields> Fields { get; set; }
    }
    public class ServiceFields
    {
        public string Description { get; set; }
        [JsonConverter(typeof(JsonGenericDeserializer))]
        public string Example { get; set; }

        public override string ToString()
        {
            return Description;
        }
    }
}
