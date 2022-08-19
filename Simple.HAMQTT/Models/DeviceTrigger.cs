using Newtonsoft.Json;

namespace Simple.HAMQTT.Models
{
    public class DeviceTrigger : DeviceRegistry
    {
        // https://www.home-assistant.io/integrations/device_trigger.mqtt/
        public DeviceTrigger()
        {
            Component = "device_automation";
            AutomationType = "trigger";
        }

        [JsonProperty("automation_type", NullValueHandling = NullValueHandling.Ignore)]
        public string AutomationType { get; set; }

        [JsonProperty("topic", NullValueHandling = NullValueHandling.Ignore)]
        public string Topic { get; set; }

        [JsonProperty("type", NullValueHandling = NullValueHandling.Ignore)]
        public string Type { get; set; }
        [JsonProperty("subtype", NullValueHandling = NullValueHandling.Ignore)]
        public string SubType { get; set; }

        public static DeviceTrigger Build(string topic, int buttonNumber, string serialNumber, string deviceId)
        {
            return new DeviceTrigger()
            {
                Topic = topic,
                Type = "button_short_press",
                SubType = $"button_{buttonNumber}",
                Device = new DeviceInfo()
                {
                    Identifiers = new string[] { serialNumber },
                },
                DeviceId = deviceId,
            };
        }
    }
}
