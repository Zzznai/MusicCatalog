using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.DTOs.Requests;
using MusicCatalog.Api.DTOs.Responses;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Services;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PlaylistController:ControllerBase
{
    private readonly PlaylistService _playlistService;

    public PlaylistController(PlaylistService playlistService)
    {
        _playlistService = playlistService;
    }


    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var playlists = await _playlistService.GetAll();

        var result = playlists.Select(PlaylistResponse.FromEntity);

        return Ok(result);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var playlist = await _playlistService.GetById(id);

        if (playlist == null) return NotFound("Playlist not found.");

        return Ok(PlaylistResponse.FromEntity(playlist));
    }

    [HttpGet("user/{userId}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByUserId(int userId)
    {
        var playlists = await _playlistService.GetByUserId(userId);

        var result = playlists.Select(PlaylistResponse.FromEntity);

        return Ok(result);
    }

    [HttpGet("user/username/{username}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var playlists = await _playlistService.GetByUsername(username);

        var result = playlists.Select(PlaylistResponse.FromEntity);

        return Ok(result);
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> Create(CreatePlaylistRequest createPlaylistRequest)
    {
        var userId = int.Parse(User.FindFirst("loggedUserId")!.Value);

        var playlist = await _playlistService.Create(new Playlist
        {
            Name = createPlaylistRequest.Name,
            UserId = userId
        });

        if(playlist == null) return NotFound("User not found.");

        return Ok(PlaylistResponse.FromEntity(playlist));
    }

    [HttpPut("{id}")]
    [Authorize]
    public async Task<IActionResult> Update(int id, UpdatePlaylistRequest updatePlaylistRequest)
    {
        var userId = int.Parse(User.FindFirst("loggedUserId")!.Value);

        var existing = await _playlistService.Update(id, userId, updatePlaylistRequest.Name);

        if(existing == null) return NotFound("Playlist not found or access denied.");

        return Ok(PlaylistResponse.FromEntity(existing));
    }

    [HttpDelete("{id}")]
    [Authorize]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirst("loggedUserId")!.Value);
        
        var deleted = await _playlistService.Delete(id, userId);

        if(deleted == false) return NotFound("Playlist not found or access denied.");

        return NoContent();

    }

    [HttpPut("{playlistId}/song/{songId}")]
    [Authorize]
    public async Task<IActionResult> AddSong(int playlistId, int songId)
    {
        var userId = int.Parse(User.FindFirst("loggedUserId")!.Value);

        var playlist = await _playlistService.GetById(playlistId);
        if (playlist == null) return NotFound("Playlist not found.");
        if (PlaylistService.HasSong(playlist, songId))
            return BadRequest("Playlist already contains this song.");

        if (!await _playlistService.AddSong(playlistId, userId, songId))
            return NotFound("Song not found or access denied.");

        playlist = await _playlistService.GetById(playlistId);
        return Ok(PlaylistResponse.FromEntity(playlist));
    }

    [HttpDelete("{playlistId}/song/{songId}")]
    [Authorize]
    public async Task<IActionResult> RemoveSong(int playlistId, int songId)
    {
        var userId = int.Parse(User.FindFirst("loggedUserId")!.Value);

        var playlist = await _playlistService.GetById(playlistId);
        if (playlist == null) return NotFound("Playlist not found.");
        if (!PlaylistService.HasSong(playlist, songId))
            return BadRequest("Playlist does not contain this song.");

        if (!await _playlistService.RemoveSong(playlistId, userId, songId))
            return NotFound("Song not found or access denied.");

        return NoContent();
    }

}