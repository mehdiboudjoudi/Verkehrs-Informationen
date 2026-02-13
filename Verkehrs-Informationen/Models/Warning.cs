using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Verkehrs_Informationen.Models
{
    public class Warning
    {
        [JsonPropertyName("warning")] public List<WarningItem>? Warnings { get; set; }
    }

    public class Closure
    {
        [JsonPropertyName("closure")] public List<WarningItem>? Closures { get; set; }
    }

    public class WarningItem
    {
        [JsonPropertyName("title")]
        public string Title { get; set; } = "";

        [JsonPropertyName("subtitle")]
        public string Subtitle { get; set; } = "";

        [JsonPropertyName("isBlocked")]
        public string IsBlocked { get; set; } = "false";

        [JsonPropertyName("description")]
        public List<string> DescriptionList { get; set; } = new();

        [JsonIgnore]
        public string FullDescription => DescriptionList != null ? string.Join("\n", DescriptionList) : "";

        [JsonIgnore]
        public string BlockedStatus => IsBlocked == "true" ? "Gesperrt ⛔" : "Frei ✅";
    }
}