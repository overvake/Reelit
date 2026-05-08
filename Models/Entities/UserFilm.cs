namespace Reelit.Models.Entities;

public class UserFilm
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public int FilmId { get; set; }
    public Film Film { get; set; }
    public string? Status { get; set; }
    public double? UserRating { get; set; }
    public string? Comment { get; set; }
    public DateTime AddedAt { get; set; } = DateTime.UtcNow;
}