using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Reelit.Data;
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
        return await _context.Films.ToListAsync();
    }

    [HttpPost]
    public async Task<ActionResult<Film>> AddFilm(Film film)
    {
        _context.Films.Add(film);
        await _context.SaveChangesAsync();
        return CreatedAtAction(nameof(GetFilms), new { id = film.Id }, film);
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
}
