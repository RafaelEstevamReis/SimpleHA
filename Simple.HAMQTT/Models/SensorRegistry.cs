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

        public const string CLASS_NONE = "None";
        public const string CLASS_APPARENT_POWER = "apparent_power";
        public const string CLASS_AIRQUALITYINDX = "aqi";
        public const string CLASS_BATTERY = "battery";
        public const string CLASS_CARBON_DIOXIDE = "carbon_dioxide";
        public const string CLASS_CARBON_MONOXIDE = "carbon_monoxide";
        public const string CLASS_CURRENT = "current";
        public const string CLASS_DATE = "date";
        public const string CLASS_DURATION = "duration";
        public const string CLASS_ENERGY = "energy";
        public const string CLASS_FREQUENCY = "frequency";
        public const string CLASS_GAS = "gas";
        public const string CLASS_HUMIDITY = "humidity";
        public const string CLASS_ILLUMINANCE = "illuminance";
        public const string CLASS_MONETARY = "monetary";
        public const string CLASS_NITROGEN_DIOXIDE = "nitrogen_dioxide";
        public const string CLASS_NITROGEN_MONOXIDE = "nitrogen_monoxide";
        public const string CLASS_NITROUS_OXIDE = "nitrous_oxide";
        public const string CLASS_OZONE = "ozone";
        public const string CLASS_PM1 = "pm1";
        public const string CLASS_PM10 = "pm10";
        public const string CLASS_PM25 = "pm25";
        public const string CLASS_POWER_FACTOR = "power_factor";
        public const string CLASS_POWER = "power";
        public const string CLASS_PRESSURE = "pressure";
        public const string CLASS_REACTIVE_POWER = "reactive_power";
        public const string CLASS_SIGNAL_STRENGTH = "signal_strength";
        public const string CLASS_SULPHUR_DIOXIDE = "sulphur_dioxide";
        public const string CLASS_TEMPERATURE = "temperature";
        public const string CLASS_TIMESTAMP = "timestamp";
        public const string CLASS_VOLATILE_ORGANIC_COMPOUNDS = "volatile_organic_compounds";
        public const string CLASS_VOLTAGE = "voltage";

    }
}
