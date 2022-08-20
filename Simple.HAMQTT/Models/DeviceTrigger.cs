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

        public const string TYPE_BUTTON_SHORT_PRESS = "button_short_press";
        public const string TYPE_BUTTON_SHORT_RELEASE = "button_short_release";
        public const string TYPE_BUTTON_LONG_PRESS = "button_long_press";
        public const string TYPE_BUTTON_LONG_RELEASE = "button_long_release";
        public const string TYPE_BUTTON_DOUBLE_PRESS = "button_double_press";
        public const string TYPE_BUTTON_TRIPLE_PRESS = "button_triple_press";
        public const string TYPE_BUTTON_QUADRUPLE_PRESS = "button_quadruple_press";
        public const string TYPE_BUTTON_QUINTUPLE_PRESSS = "button_quintuple_press";

        public const string SUBTYPE_TURN_ON = "turn_on";
        public const string SUBTYPE_TURN_OFF = "turn_off";
        public const string SUBTYPE_BUTTON_1 = "button_1";
        public const string SUBTYPE_BUTTON_2 = "button_2";
        public const string SUBTYPE_BUTTON_3 = "button_3";
        public const string SUBTYPE_BUTTON_4 = "button_4";
        public const string SUBTYPE_BUTTON_5 = "button_5";
        public const string SUBTYPE_BUTTON_6 = "button_6";

    }
}
