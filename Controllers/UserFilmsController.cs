using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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

    [HttpGet]
    public async Task<ActionResult<List<OmdbDto>>> Search(string film)
    {
        var result = await _omdb.SeachFilmsAsync(film);
        if (result == null) return NotFound();
        return Ok(result);
    }
    
    // [HttpPost]
    // public async Task<ActionResult<Film>> AddFilm (int tmbdId) {}
    //
    // [HttpPut("{id}/rate")]
    // public async Task<ActionResult<int>> RateFilm (int rate) {}
    //
    // [HttpPut("{id}/note")]
    // public async Task<ActionResult<string>> NoteFilm (string note) {}
    //
    // [HttpDelete]
    // public async Task<IActionResult> DeleteFilm (int id) {}
}