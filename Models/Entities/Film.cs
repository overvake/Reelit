namespace  Reelit.Models.Entities;

public class Film
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Overview { get; set; }
    public string? PosterPath { get; set; }
    public string? Genre { get; set; }
    public string ImdbId { get; set; } = string.Empty;
    public string ReleaseDate { get; set; }
    
}
