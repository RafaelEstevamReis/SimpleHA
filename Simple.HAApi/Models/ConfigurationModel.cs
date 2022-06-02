using Newtonsoft.Json;
using System.Collections.Generic;

namespace Simple.HAApi.Models
{
    public class ConfigurationModel
    {
        public List<string> Components { get; set; }
        [JsonProperty("config_dir")]
        public string ConfigDirectory { get; set; }
        [JsonProperty("config_source")]
        public string ConfigSource { get; set; }
        public int Elevation { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        [JsonProperty("location_name")]
        public string LocationName { get; set; }
        [JsonProperty("time_zone")]
        public string TimeZone { get; set; }
        [JsonProperty("unit_system")]
        public UnitSystemModel UnitSystem { get; set; }
        public string Version { get; set; }
        [JsonProperty("whitelist_external_dirs")]
        public List<string> WhitelistedExternalDirs { get; set; }
    }
}
