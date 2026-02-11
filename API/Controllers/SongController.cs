using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.DTOs.Requests;
using MusicCatalog.Api.DTOs.Responses;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Services;

namespace MusicCatalog.Api.Controllers;


[ApiController]
[Route("api/[controller]")]
public class SongController:ControllerBase
{
    private readonly SongService _songService;
    public SongController(SongService songService)
    {
        _songService = songService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var songs = await _songService.GetAll();

        var response = songs.Select(SongResponse.FromEntity);

        return Ok(response);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetbyId(int id)
    {
        var song = await _songService.GetById(id);
        
        if(song == null) return NotFound("Song not found.");

        return Ok(SongResponse.FromEntity(song));
    }

/*
public string Title { get; set; } = string.Empty;
    public TimeSpan Duration { get; set; }
    public int ArtistId { get; set; }
    public int? AlbumId { get; set; }
*/
    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateSongRequest createSongRequest)
    {
        var song = await _songService.Create(new Song
        {
            Title = createSongRequest.Title,
            Duration = createSongRequest.Duration,
            ArtistId = createSongRequest.ArtistId,
            AlbumId = createSongRequest.AlbumId
        });

        if(song == null) return NotFound("Artist or Album not found.");

        return Ok(SongResponse.FromEntity(song));

    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateSongRequest updateSongRequest)
    {
        var updated = await _songService.Update(id, new Song
        {
            Title = updateSongRequest.Title,
            Duration = updateSongRequest.Duration,
            ArtistId = updateSongRequest.ArtistId,
            AlbumId = updateSongRequest.AlbumId
        });

        if(updated == null) return NotFound("Song, Artist or Album not found.");

        return Ok(SongResponse.FromEntity(updated));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _songService.Delete(id);

        if(!deleted) return NotFound("Song not found.");

        return NoContent();
    }

    [HttpPut("{songId}/genre/{genreId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddGenre(int songId, int genreId)
    {
        var song = await _songService.GetById(songId);
        if (song == null) return NotFound("Song not found.");
        if (SongService.HasGenre(song, genreId))
            return BadRequest("Song already has this genre.");

        if (!await _songService.AddGenre(songId, genreId))
            return NotFound("Genre not found.");

        song = await _songService.GetById(songId);
        return Ok(SongResponse.FromEntity(song));
    }

    [HttpDelete("{songId}/genre/{genreId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveGenre(int songId, int genreId)
    {
        var song = await _songService.GetById(songId);
        if (song == null) return NotFound("Song not found.");
        if (!SongService.HasGenre(song, genreId))
            return BadRequest("Song does not have this genre.");

        if (!await _songService.RemoveGenre(songId, genreId))
            return NotFound("Genre not found.");

        return NoContent();
    }
}