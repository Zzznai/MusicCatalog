using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Services;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GenreController : ControllerBase
{
    private readonly GenreService _genreService;

    public GenreController(GenreService genreService)
    {
        _genreService = genreService;
    }

    [HttpGet]
    public async Task<ActionResult<List<Genre>>> GetAll()
    {
        return await _genreService.GetAll();
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<Genre>> GetById(int id)
    {
        var genre = await _genreService.GetById(id);
        if (genre == null) return NotFound();
        return genre;
    }

    [HttpPost]
    public async Task<ActionResult<Genre>> Create(Genre genre)
    {
        var created = await _genreService.Create(genre);
        return CreatedAtAction(nameof(GetById), new { id = created.Id }, created);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<Genre>> Update(int id, Genre genre)
    {
        var updated = await _genreService.Update(id, genre);
        if (updated == null) return NotFound();
        return updated;
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _genreService.Delete(id);
        if (!deleted) return NotFound();
        return NoContent();
    }
}