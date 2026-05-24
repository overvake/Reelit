using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reelit.Data;
using Reelit.Models.DTOs;
using Reelit.Models.Entities;
using Reelit.Services;
using Reelit.Services.Interfaces;

namespace Reelit.Controllers;
[ApiController]
[Authorize]
[Route("api/[controller]")]
public class UserFilmsController : ControllerBase
{

    private readonly AppDbContext _context;
    private readonly IOmdbService _omdb;

    public UserFilmsController(AppDbContext context, IOmdbService omdbService)
    {
        _context = context;
        _omdb = omdbService;
    }

    [HttpGet("search")]
    public async Task<ActionResult<List<OmdbDto>>> Search(string film)
    {
        var result = await _omdb.SeachFilmsAsync(film);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("film/{imdbId}")]
    public async Task<ActionResult<OmdbDto>> GetById(string imdbId)
    {
        var result = await _omdb.GetById(imdbId);
        if (result == null) return NotFound();
        return Ok(result);
    }

    [HttpGet("catalog")]
    public async Task<ActionResult<List<UserFilm>>> GetCatalog()
    {
        var userId = GetUserId();
        var film = await _context.UserFilms
            .Where(x => x.UserId == userId)
            .Include(x => x.Film)
            .ToListAsync();
        return Ok(film.Select(MapToDto));
    }

    [HttpPost("add")]
    public async Task<ActionResult<Film>> AddFilm([FromQuery] string imbdId)
    {
        Film film = await _context.Films.FirstOrDefaultAsync(f => f.ImdbId == imbdId);
        if (film == null)
        {
            var newFilm = await _omdb.GetById(imbdId);
            if (newFilm == null) return NotFound();
            film = new Film
            {
                ImdbId = newFilm.imdbId,
                Title = newFilm.Title,
                Genre = newFilm.Genre,
                Overview = newFilm.Plot,
                PosterPath = newFilm.PosterPath,
                ReleaseDate = newFilm.Year
            };
            await _context.Films.AddAsync(film);
            await _context.SaveChangesAsync();

        }
            
        var userId = GetUserId();
        if (await _context.UserFilms.AnyAsync(x => x.UserId == userId && x.FilmId == film.Id))
            return Conflict(new {message = "Film already in catalog"});
        
        UserFilm userFilm = new UserFilm
        {
            UserId = userId,
            FilmId = film.Id
        };

        await _context.UserFilms.AddAsync(userFilm);
        await _context.SaveChangesAsync();
        
        return Ok(MapFilmToDto(film));
    }

    [HttpPatch("{id}")]
    public async Task<ActionResult<int>> UpdateFilm(int id, [FromBody] UpdateUserFilmDto dto)
    {
        var userId = GetUserId();
        var userFilm = await _context.UserFilms
            .FirstOrDefaultAsync(x => x.UserId == userId && x.FilmId == id);

        if (userFilm == null) return NotFound();

        if (dto.UserRating.HasValue) userFilm.UserRating = dto.UserRating;
        if (dto.Comment != null) userFilm.Comment = dto.Comment;
        if (dto.Status != null) userFilm.Status = dto.Status;

        await _context.SaveChangesAsync();
        return NoContent();

    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteFilm(int id)
    {
        var userId = GetUserId();
        var film = await _context.UserFilms
            .FirstOrDefaultAsync(film => film.Id == id && userId == film.UserId);
       
        if (film == null) return NotFound();
        _context.UserFilms.Remove(film);
        await _context.SaveChangesAsync();
        return NoContent();
    }
    
    
    private int GetUserId() =>
        int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
    
    private static UserFilmDto MapToDto(UserFilm uf) => new()
    {
        Id = uf.Id,
        Film = MapFilmToDto(uf.Film),
        Status = uf.Status,
        UserRating = uf.UserRating,
        Comment = uf.Comment,
        AddedAt = uf.AddedAt
    };

    private static FilmDto MapFilmToDto(Film film) => new()
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