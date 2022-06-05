using Newtonsoft.Json;

namespace Simple.HAMQTT.Models
{
    public class ButtonRegistry : DeviceRegistry
    {
        public ButtonRegistry()
        {
            Component = "button";
        }

        [JsonProperty("command_topic", NullValueHandling = NullValueHandling.Ignore)]
        public string CommandTopic { get; set; }
        [JsonProperty("payload_press ", NullValueHandling = NullValueHandling.Ignore)]
        public string PayloadPress { get; set; }

        public static ButtonRegistry Build(string displayName, string commandTopic, string payloadPress, string deviceId, string availabilityTopic = null)
        {
            return new ButtonRegistry
            {
                Name = displayName,
                CommandTopic = commandTopic,
                PayloadPress = payloadPress,
                DeviceId = deviceId,
                AvailabilityTopic = availabilityTopic,
            };
        }
    }
}
