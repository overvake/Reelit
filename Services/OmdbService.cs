using Microsoft.AspNetCore.Http.HttpResults;
using Reelit.Models.DTOs;
using Reelit.Services.Interfaces;

namespace Reelit.Services;

public class OmdbService : IOmdbService
{
    private HttpClient _httpClient;
    private IConfiguration _config;

    public OmdbService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _config = config;

    }
    public async Task<List<OmdbDto>> SeachFilmsAsync(string filmName)
    {
        // var result = await _httpClient.GetFromJsonAsync<OmdbDto>();
        var response = await _httpClient.GetAsync($"?s={filmName}&apikey={_config["Omdb:ApiKey"]}");
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadFromJsonAsync<OmdbSeachResponseDto>();
            if (data == null) return new List<OmdbDto>();
            return data.Search;
        }
        else
        {
            return new List<OmdbDto>();
        }
    }
}