using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Verkehrs_Informationen.APIs;

public class AutobahnService
{
    private static readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://autobahn.api.bund.dev/roadmaps/")
    };

    public async Task<List<WarningItem>?> GetWarningsAsync(string roadId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<WarningResponse>($"{roadId}/services/warning");
            return response?.Warning ?? new List<WarningItem>();
        }
        catch (Exception ex)        {
            Console.WriteLine($"Fehler beim Abruf: {ex.Message}");
            return null;
        }
    }
}