using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Verkehrs_Informationen.Models;
using System.Diagnostics;

namespace Verkehrs_Informationen.APIs;

public class AutobahnAPI
{
    private static readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://verkehr.autobahn.de/o/autobahn/")
    };

    public async Task<Road> GetRoads()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<Road>("");
            if (response != null)
            {
                Debug.WriteLine($"Anzahl Straßen: {response.Roads.Count}");
                return response;
            }
            else
            {
                Debug.WriteLine("API Antwort erhalten, aber Straßenliste war null oder leer.");
                return new Road();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Fehler beim Abruf: {ex.Message}");
            return new Road();
        }
    }

    public async Task<List<WarningItem>?> GetWarnings(string roadId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<Warning>($"{roadId}/services/warning");
            if (response?.Warnings != null)
            {
                Debug.WriteLine($"Anzahl Warnungen: {response.Warnings.Count}");
                return response.Warnings;
            }
            else
            {
                Debug.WriteLine("API Antwort erhalten, aber Warnings-Liste war null oder leer.");
                return new List<WarningItem>();
            }

            return response?.Warnings;
        }
        catch (Exception ex)        {
            Console.WriteLine($"Fehler beim Abruf: {ex.Message}");
            return null;
        }
    }
}