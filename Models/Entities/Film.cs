namespace  Reelit.Models.Entities;

public class Film
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Overview { get; set; }
    public string? PosterPath { get; set; }
    public int TmdbId { get; set; }
    public int? UserRating { get; set; }
    public string Status { get; set; } = "Planned";
    public string? Comment { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
    
}
