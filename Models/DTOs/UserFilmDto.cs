namespace Reelit.Models.DTOs;

public class UserFilmDto
{
    public int Id { get; set; }
    public FilmDto Film { get; set; } = null!;
    public string? Status { get; set; }
    public double? UserRating { get; set; }
    public string? Comment { get; set; }
    public DateTime AddedAt { get; set; }

}