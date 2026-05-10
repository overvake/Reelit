using Microsoft.AspNetCore.Mvc;
using Reelit.Models.DTOs;

namespace Reelit.Services.Interfaces;

public interface IOmdbService
{
    public Task<List<OmdbDto>> SeachFilmsAsync(string filmName);
    public Task<OmdbDto> GetById(string imdbId);
}