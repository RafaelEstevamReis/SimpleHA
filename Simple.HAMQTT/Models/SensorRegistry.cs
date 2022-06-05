using Newtonsoft.Json;

namespace Simple.HAMQTT.Models
{
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
}
