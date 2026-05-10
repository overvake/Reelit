using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using Reelit.Models.Entities;

namespace Reelit.Models.DTOs;

public class OmdbSeachResponseDto
{
    [Required][JsonPropertyName("totalResults")] public int Results { get; set; }
    [Required][JsonPropertyName("Search")] public List<OmdbDto> Search { get; set; }
    [Required][JsonPropertyName("Response")] public string Response { get; set; }
    
}