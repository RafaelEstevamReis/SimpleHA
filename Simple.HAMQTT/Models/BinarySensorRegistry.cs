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

        public const string CLASS_NONE = "None";
        public const string CLASS_BATTERY = "battery";
        public const string CLASS_BATTERY_CHARGING = "battery_charging";
        public const string CLASS_CARBON_MONOXIDE = "carbon_monoxide";
        public const string CLASS_COLD = "cold";
        public const string CLASS_CONNECTIVITY = "connectivity";
        public const string CLASS_DOOR = "door";
        public const string CLASS_GARAGE_DOOR = "garage_door";
        public const string CLASS_GAS = "gas";
        public const string CLASS_HEAT = "heat";
        public const string CLASS_LIGHT = "light";
        public const string CLASS_LOCK = "lock";
        public const string CLASS_MOISTURE = "moisture";
        public const string CLASS_MOTION = "motion";
        public const string CLASS_MOVING = "moving";
        public const string CLASS_OCCUPANCY = "occupancy";
        public const string CLASS_OPENING = "opening";
        public const string CLASS_PLUG = "plug";
        public const string CLASS_POWER = "power";
        public const string CLASS_PRESENCE = "presence";
        public const string CLASS_PROBLEM = "problem";
        public const string CLASS_RUNNING = "running";
        public const string CLASS_SAFETY = "safety";
        public const string CLASS_SMOKE = "smoke";
        public const string CLASS_SOUND = "sound";
        public const string CLASS_TAMPER = "tamper";
        public const string CLASS_UPDATE = "update";
        public const string CLASS_VIBRATION = "vibration";
        public const string CLASS_WINDOW = "window";

    }
}
