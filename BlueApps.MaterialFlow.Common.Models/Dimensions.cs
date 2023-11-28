using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace BlueApps.MaterialFlow.Common.Models
{
    public class Dimensions
    {
        [JsonIgnore]
        public int Id { get; set; }
        [JsonPropertyName("should_weight")]
        public double ShouldWeight { get; set; }
        [JsonPropertyName("is_weight")]
        public double IsWeight { get; set; }
        [JsonPropertyName("weight_tolerance")]
        public double WeightTolerance { get; set; }
        [JsonIgnore]
        public double ShouldLength { get; set; }
        [JsonIgnore]
        public double IsLength { get; set; }
        [JsonIgnore]
        public double LengthTolerance { get; set; }
        [JsonIgnore]
        public double ShouldWidth { get; set; }
        [JsonIgnore]
        public double IsWidth { get; set; }
        [JsonIgnore]
        public double WidthTolerance { get; set; }
        [JsonIgnore]
        public double ShouldHeight { get; set; }
        [JsonIgnore]
        public double IsHeight { get; set; }
        [JsonIgnore]
        public double HeightTolerance { get; set; }
    }
}
