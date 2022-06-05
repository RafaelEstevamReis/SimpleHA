using Newtonsoft.Json;

namespace Simple.HAMQTT.Models
{
    public class BinarySensorRegistry : DeviceRegistry
    {
        public BinarySensorRegistry()
        {
            Component = "binary_sensor";
        }

        [JsonProperty("state_topic", NullValueHandling = NullValueHandling.Ignore)]
        public string StateTopic { get; set; }

        [JsonProperty("payload_on", NullValueHandling = NullValueHandling.Ignore)]
        public string PayloadOn { get; set; }
        [JsonProperty("payload_off", NullValueHandling = NullValueHandling.Ignore)]
        public string PayloadOff { get; set; }

        public static BinarySensorRegistry Build(string displayName, string stateTopic, string deviceClass, string sensorId,
                                                 string payloadOn = null, string payloadOff = null,
                                                 string deviceId = null, string availabilityTopic = null)
        {
            return new BinarySensorRegistry
            {
                Name = displayName,
                StateTopic = stateTopic,
                DeviceClass = deviceClass,
                UniqueId = sensorId,

                PayloadOn = payloadOn,
                PayloadOff = payloadOff,

                DeviceId = deviceId ?? sensorId,
                AvailabilityTopic = availabilityTopic,
            };
        }
    }
}
