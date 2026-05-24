using System.ComponentModel.DataAnnotations;

namespace Reelit.Models.DTOs;

public class UpdateUserFilmDto
{
    [Range(1, 10, ErrorMessage = "Rating must be between 1 and 10")]
    public double? UserRating { get; set; }
 
    [MaxLength(1000, ErrorMessage = "Comment must not exceed 1000 characters")]
    public string? Comment { get; set; }
 
    public string? Status { get; set; }

}