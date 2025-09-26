namespace Simple.HAApi.Models;

using Newtonsoft.Json;
using System.Collections.Generic;

public class ConfigurationModel
{
    public List<string> Components { get; set; }
    [JsonProperty("config_dir")]
    public string ConfigDirectory { get; set; }
    [JsonProperty("config_source")]
    public string ConfigSource { get; set; }
    public string State { get; set; }
    public string Country { get; set; }
    public string Currency { get; set; }
    public bool Debug { get; set; }
    [JsonProperty("recovery_mode")]
    public bool RecoveryMode { get; set; }
    [JsonProperty("safe_mode")]
    public bool SafeMode { get; set; }
    public int Elevation { get; set; }
    public decimal Latitude { get; set; }
    public decimal Longitude { get; set; }
    public decimal Radius { get; set; }
    [JsonProperty("location_name")]
    public string LocationName { get; set; }
    public string Language { get; set; }
    [JsonProperty("time_zone")]
    public string TimeZone { get; set; }
    [JsonProperty("unit_system")]
    public UnitSystemModel UnitSystem { get; set; }
    public string Version { get; set; }
    [JsonProperty("whitelist_external_dirs")]
    public List<string> WhitelistedExternalDirs { get; set; }
    [JsonProperty("allowlist_external_urls")]
    public List<string> AllowlistedExternalUrls { get; set; }
    [JsonProperty("external_url")]
    public string ExternalUrl { get; set; }
    [JsonProperty("internal_url")]
    public string InternalUrl { get; set; }
}
