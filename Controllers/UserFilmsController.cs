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
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var films = await _context.UserFilms.Where(x => x.UserId == int.Parse(userId))
            .Include(x => x.Film)
            .ToListAsync();
        return Ok(films);
    }

    [HttpPost("add")]
    public async Task<ActionResult<Film>> AddFilm(string imbdId)
    {
        Film film = await _context.Films.FirstOrDefaultAsync(f => f.ImdbId == imbdId);
        if (film == null)
        {
            var newFilm = await _omdb.GetById(imbdId);
            if (newFilm == null) return NotFound();
            film = new Film(); 
            film.ImdbId = newFilm.imdbId;
            film.Title = newFilm.Title;
            film.Genre = newFilm.Genre;
            film.Overview = newFilm.Plot;
            film.PosterPath = newFilm.PosterPath;
            film.ReleaseDate = newFilm.Year;
            await _context.Films.AddAsync(film);
            await _context.SaveChangesAsync();
        }
            
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (await _context.UserFilms.AnyAsync(x => x.UserId == int.Parse(userId) && x.FilmId == film.Id))
            return Conflict();
        
        UserFilm userFilm = new();
        userFilm.UserId = int.Parse(userId);
        userFilm.FilmId = film.Id;

        await _context.UserFilms.AddAsync(userFilm);
        await _context.SaveChangesAsync();
        return Ok(film);
    }

    [HttpPut("{id}/rate")]
    public async Task<ActionResult<int>> RateFilm(int id, int rate)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var film = await _context.UserFilms.FirstOrDefaultAsync(x => x.UserId == int.Parse(userId) && x.FilmId == id);
        if (film == null) return NotFound();
        film.UserRating = rate;
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpPut("{id}/note")]
    public async Task<ActionResult<string>> NoteFilm(int id, string note)
    {
        var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var film = await _context.UserFilms.FirstOrDefaultAsync(x => x.UserId == int.Parse(userId) && x.FilmId == id);
        if (film == null) return NotFound();
        film.Comment = note;
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpDelete]
    public async Task<IActionResult> DeleteFilm(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier));
        var film = await _context.UserFilms.FirstOrDefaultAsync(film => film.Id == id && userId == film.UserId);
        if (film == null) return NotFound();
        _context.UserFilms.Remove(film);
        await _context.SaveChangesAsync();
        return NoContent();
    }
}