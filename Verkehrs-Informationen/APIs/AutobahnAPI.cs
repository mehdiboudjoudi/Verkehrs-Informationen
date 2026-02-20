using System;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Collections.Generic;
using Verkehrs_Informationen.Models;
using System.Diagnostics;

namespace Verkehrs_Informationen.APIs;

/// <summary>
/// Provides access to the Autobahn traffic API to retrieve road information,
/// warnings, closures, and road works.
/// </summary>
public class AutobahnAPI
{
    /// <summary>
    /// Static HTTP client instance for API requests with base address configured.
    /// </summary>
    private static readonly HttpClient _httpClient = new HttpClient
    {
        BaseAddress = new Uri("https://verkehr.autobahn.de/o/autobahn/")
    };

    /// <summary>
    /// Retrieves all available roads from the API.
    /// </summary>
    /// <returns>Road object containing list of roads, or empty Road object if failed.</returns>
    public async Task<Road> GetRoads()
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<Road>("");
            if (response != null)
            {
                Debug.WriteLine($"Number of roads: {response.Roads.Count}");
                return response;
            }
            else
            {
                Debug.WriteLine("API response received, but roads list was null or empty.");
                return new Road();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving roads: {ex.Message}");
            return new Road();
        }
    }

    /// <summary>
    /// Retrieves warnings for a specific road.
    /// </summary>
    /// <param name="roadId">The road identifier (e.g., "A1", "A2").</param>
    /// <returns>List of warning items for the specified road, or null if an error occurs.</returns>
    public async Task<List<WarningItem>?> GetWarnings(string roadId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<Warning>($"{roadId}/services/warning");
            if (response?.Warnings != null)
            {
                Debug.WriteLine($"Number of warnings: {response.Warnings.Count}");
                return response.Warnings;
            }
            else
            {
                Debug.WriteLine("API response received, but warnings list was null or empty.");
                return new List<WarningItem>();
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error retrieving warnings: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Retrieves closed roads information for a specific road.
    /// </summary>
    /// <param name="roadId">The road identifier.</param>
    /// <returns>List of closed road items, or empty list if none found.</returns>
    public async Task<List<WarningItem>?> GetClosedRoads(string roadId)
    {
        try
        {
            // Correct API endpoint path for road closures
            var response = await _httpClient.GetFromJsonAsync<ClosedRoad>($"{roadId}/services/closure");
            return response?.ClosedRoads ?? new List<WarningItem>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving road closures: {ex.Message}");
            return null;
        }
    }

    /// <summary>
    /// Retrieves road works information for a specific road.
    /// </summary>
    /// <param name="roadId">The road identifier.</param>
    /// <returns>List of road work items, or empty list if none found.</returns>
    public async Task<List<WarningItem>?> GetRoadWorks(string roadId)
    {
        try
        {
            var response = await _httpClient.GetFromJsonAsync<RoadWork>($"{roadId}/services/roadworks");
            return response?.RoadWorks ?? new List<WarningItem>();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error retrieving road works: {ex.Message}");
            return null;
        }
    }
}