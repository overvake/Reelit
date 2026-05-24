using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reelit.Data;
using Reelit.Models.DTOs;
using Reelit.Models.Entities;

namespace Reelit.Controllers;

[ApiController]
[Authorize]
[Route("api/[controller]")]
public class FilmsController : ControllerBase
{
    private readonly AppDbContext _context;

    public FilmsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<Film>>> GetFilms()
    {
        var films = await _context.Films.ToListAsync();
        return Ok(films.Select(MapToDto));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteFilm(int id)
    {
        var film = await _context.Films.FindAsync(id);
        if (film == null) return NotFound();
        _context.Films.Remove(film);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    private static FilmDto MapToDto(Film film) => new()
    {
        Id = film.Id,
        Title = film.Title,
        Overview = film.Overview,
        PosterPath = film.PosterPath,
        Genre = film.Genre,
        ImdbId = film.ImdbId,
        ReleaseDate = film.ReleaseDate
    };
}
