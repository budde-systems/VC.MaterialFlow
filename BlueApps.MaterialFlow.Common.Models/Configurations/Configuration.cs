using System.Text.Json.Serialization;

namespace BlueApps.MaterialFlow.Common.Models.Configurations;

public class Configuration
{
    [JsonPropertyName("key")]
    public string? Key { get; set; }
    [JsonPropertyName("value")]
    public object? Value { get; set; }
}