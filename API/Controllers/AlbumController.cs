using System.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.DTOs.Requests;
using MusicCatalog.Api.DTOs.Responses;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Services;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlbumController:ControllerBase
{
    private readonly AlbumService _albumService;

    public AlbumController(AlbumService albumService)
    {
        _albumService = albumService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var result = await _albumService.GetAll();
        var response = result.Select(AlbumResponse.FromEntity);
        return Ok(response);
    }


    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var album = await _albumService.GetById(id);
        if(album == null) return NotFound();

        return Ok(AlbumResponse.FromEntity(album));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateAlbumRequest createAlbumRequest)
    {
        var album = await _albumService.Create(new Album
        {
            Name = createAlbumRequest.Name,
            Description = createAlbumRequest.Description,
            ArtistId = createAlbumRequest.ArtistId
        });

        if(album == null)
           return NotFound();
        
        return Ok(AlbumResponse.FromEntity(album));
    }


    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateAlbumRequest updateAlbumRequest)
    {
        var album = await _albumService.Update(id, new Album
        {
            Name = updateAlbumRequest.Name,
            Description = updateAlbumRequest.Description,
            ArtistId = updateAlbumRequest.ArtistId
        });

         if(album == null)
           return NotFound();
        
        return Ok(AlbumResponse.FromEntity(album));
    }


    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _albumService.Delete(id);

        if(!deleted)
          return NotFound(); 

        return NoContent();
    }

    [HttpPut("{albumId}/song/{songId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddSong(int albumId, int songId)
    {
        var album = await _albumService.GetById(albumId);
        if (album == null) return NotFound();
        if (AlbumService.HasSong(album, songId))
            return BadRequest("Album already contains this song.");

        var added = await _albumService.AddSong(albumId, songId);
        if(added == true)
        {
            album = await _albumService.GetById(albumId);
            return Ok(AlbumResponse.FromEntity(album));
        }

        return NotFound();
    }

    [HttpDelete("{albumId}/song/{songId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveSong(int albumId, int songId)
    {
        var album = await _albumService.GetById(albumId);
        if (album == null) return NotFound();
        if (!AlbumService.HasSong(album, songId))
            return BadRequest("Album does not contain this song.");

        var deleted = await _albumService.RemoveSong(albumId, songId);
        if(deleted == true)
        {
            album = await _albumService.GetById(albumId);
            return Ok(AlbumResponse.FromEntity(album));
        }

        return NotFound();
    }

    [HttpPut("{albumId}/mood/{moodId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddMood(int albumId, int moodId)
    {
        var album = await _albumService.GetById(albumId);
        if (album == null) return NotFound();
        if (AlbumService.HasMood(album, moodId))
            return BadRequest("Album already contains this mood.");

        var added = await _albumService.AddMood(albumId, moodId);
        if(added == true)
        {
            album = await _albumService.GetById(albumId);
            return Ok(AlbumResponse.FromEntity(album));
        }

        return NotFound();
    }

    [HttpDelete("{albumId}/mood/{moodId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveMood(int albumId, int moodId)
    {
        var album = await _albumService.GetById(albumId);
        if (album == null) return NotFound();
        if (!AlbumService.HasMood(album, moodId))
            return BadRequest("Album does not contain this mood.");

        var deleted = await _albumService.RemoveMood(albumId, moodId);
        if(deleted == true)
        {
            album = await _albumService.GetById(albumId);
            return Ok(AlbumResponse.FromEntity(album));
        }

        return NotFound();
    }


}