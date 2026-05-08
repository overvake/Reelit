using Microsoft.EntityFrameworkCore;
using Reelit.Models.Entities;

namespace Reelit.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        :base(options) {}
    public DbSet<Film> Films { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserFilm> UserFilms { get; set; }
}

