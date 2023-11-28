using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Models.Configurations
{
    public class Configuration
    {
        [JsonPropertyName("key")]
        public string? Key { get; set; }
        [JsonPropertyName("value")]
        public object? Value { get; set; }
    }
}
