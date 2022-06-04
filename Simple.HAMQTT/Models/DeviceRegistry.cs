using Newtonsoft.Json;

namespace Simple.HAMQTT.Models
{
    public class DeviceRegistry
    {
        [JsonIgnore]
        public string DeviceId { get; set; }
        [JsonIgnore]
        public string Component { get; set; }


        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("unique_id", NullValueHandling = NullValueHandling.Ignore)]
        public string UniqueId { get; set; }
        [JsonProperty("device_class", NullValueHandling = NullValueHandling.Ignore)]
        public string DeviceClass { get; set; }

        [JsonProperty("device", NullValueHandling = NullValueHandling.Ignore)]
        public DeviceInfo Device { get; set; }

        [JsonProperty("availability_topic", NullValueHandling = NullValueHandling.Ignore)]
        public string AvailabilityTopic { get; set; }
    }
    public class DeviceInfo
    {
        [JsonProperty("configuration_url", NullValueHandling = NullValueHandling.Ignore)]
        public string ConfigurationUrl { get; set; }

        [JsonProperty("connections", NullValueHandling = NullValueHandling.Ignore)]
        public string Connections { get; set; }

        [JsonProperty("identifiers", NullValueHandling = NullValueHandling.Ignore)]
        public string Identifiers { get; set; }

        [JsonProperty("name", NullValueHandling = NullValueHandling.Ignore)]
        public string Name { get; set; }

        [JsonProperty("manufacturer", NullValueHandling = NullValueHandling.Ignore)]
        public string Manufacturer { get; set; }

        [JsonProperty("model", NullValueHandling = NullValueHandling.Ignore)]
        public string Model { get; set; }
        [JsonProperty("sw_version", NullValueHandling = NullValueHandling.Ignore)]
        public string SoftwareVersion { get; set; }
        [JsonProperty("suggested_area", NullValueHandling = NullValueHandling.Ignore)]
        public string SuggestedArea { get; set; }

    }

    public class SensorRegistry : DeviceRegistry
    {
        public SensorRegistry()
        {
            Component = "sensor";
        }

        [JsonProperty("state_topic", NullValueHandling = NullValueHandling.Ignore)]
        public string StateTopic { get; set; }
        [JsonProperty("unit_of_measurement", NullValueHandling = NullValueHandling.Ignore)]
        public string UnitOfMeasurement { get; set; }

        public static SensorRegistry Build(string displayName, string stateTopic, string deviceClass, string unit, string sensorId, string deviceId = null, string availabilityTopic = null)
        {
            return new SensorRegistry
            {
                Name = displayName,
                StateTopic = stateTopic,
                DeviceClass = deviceClass,
                UnitOfMeasurement = unit,
                UniqueId = sensorId,
                DeviceId = deviceId ?? sensorId,
                AvailabilityTopic = availabilityTopic,
            };
        }
    }
    public class BinarySensorRegistry : SensorRegistry
    {
        [JsonProperty("payload_on", NullValueHandling = NullValueHandling.Ignore)]
        public string PayloadOn { get; set; }
        [JsonProperty("payload_off", NullValueHandling = NullValueHandling.Ignore)]
        public string PayloadOff { get; set; }


        public BinarySensorRegistry()
        {
            Component = "binary_sensor";
        }
    }
}
