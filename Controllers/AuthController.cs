using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Reelit.Data;
using Reelit.Models.DTOs;
using Reelit.Models.Entities;
using BC = BCrypt.Net.BCrypt;
namespace Reelit.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly IConfiguration _config;
    private readonly string _encodingKey;
    public AuthController(IConfiguration config, AppDbContext context)
    {
        _config = config;
        _context = context;
        _encodingKey = _config["Jwt:Key"];
    }
    
    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        if (await _context.Users.AnyAsync(u => u.Login == dto.Login)) return BadRequest();
        User user = new User();
        user.Login = dto.Login;
        user.PasswordHash = BC.HashPassword(dto.Password, workFactor: 12);
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        return Ok();
    }
    
    [HttpPost("login")]
    public async Task<ActionResult<string>> Login(LoginDto dto)
    {
        var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == dto.Login);
        if (user == null || !BC.Verify(dto.Password, user.PasswordHash)) return Unauthorized();
        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Name, user.Login)
        };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_encodingKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            claims: claims,
            expires: DateTime.Now.AddHours(1),
            signingCredentials: creds
        );
        return Ok(new JwtSecurityTokenHandler().WriteToken(token));
    }
}