using System.Text.Json.Serialization;

namespace Reelit.Models.DTOs;

public class OmdbDto
{
    [JsonPropertyName("imdbID")]
    public string imdbID { get; set; }
    [JsonPropertyName("Title")]
    public string Title { get; set; }
    [JsonPropertyName("Plot")]
    public string Plot { get; set; }
    [JsonPropertyName("Poster")]
    public string PosterPath { get; set; }
    [JsonPropertyName("Year")]
    public string Year { get; set; }
    [JsonPropertyName("Genre")]
    public string Genre { get; set; }
}