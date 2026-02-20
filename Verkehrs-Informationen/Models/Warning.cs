using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Verkehrs_Informationen.Models
{
    public class Warning
    {
        [JsonPropertyName("warning")] public List<WarningItem>? Warnings { get; set; }
    }

    public class ClosedRoad
    {
        [JsonPropertyName("closure")] public List<WarningItem>? ClosedRoads { get; set; }
    }

    public class RoadWork
    {
        [JsonPropertyName("roadworks")] public List<WarningItem>? RoadWorks { get; set; }
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

        // 1. Umbenennen in 'FullDescription' (vermeidet Kollision)
        // 2. JsonIgnore hinzufügen (Serializer ignoriert dieses Feld)
        [JsonIgnore]
        public string FullDescription => DescriptionList != null ? string.Join("\n", DescriptionList) : "";

        [JsonIgnore]
        public string BlockedStatus => IsBlocked == "true" ? "Gesperrt ⛔" : "Frei ✅";
    }
}