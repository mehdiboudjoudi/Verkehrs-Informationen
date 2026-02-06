using System.Collections.Generic;
using System.Text.Json.Serialization; // WICHTIG: Diesen Namespace hinzufügen

namespace Verkehrs_Informationen.Models
{
    public class Warning
    {
        // Mapping von API "warning" auf C# "Warnings"
        [JsonPropertyName("warning")]
        public List<WarningItem>? Warnings { get; set; }
    }

    public class WarningItem
    {
        // Sicherheitshalber auch hier mappen, falls Case-Sensitivity ein Problem ist
        [JsonPropertyName("title")]
        public string Title { get; set; } = string.Empty;

        [JsonPropertyName("subtitle")]
        public string Subtitle { get; set; } = string.Empty;

        [JsonPropertyName("description")]
        public List<string>? Description { get; set; }
        // ACHTUNG: Die Autobahn API gibt 'description' oft als Array von Strings zurück, 
        // nicht als einzelnen String! Prüfe das, falls es hier knallt.
        // Wenn es doch ein String ist, lass es bei string.
    }
}