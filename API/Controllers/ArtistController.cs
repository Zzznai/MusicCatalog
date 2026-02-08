using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.DTOs.Requests;
using MusicCatalog.Api.DTOs.Responses;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Services;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ArtistController : ControllerBase
{
    private readonly ArtistService _artistService;

    public ArtistController(ArtistService artistService)
    {
        _artistService = artistService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var artists = await _artistService.GetAll();
        var response = artists.Select(ArtistResponse.FromEntity);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var artist = await _artistService.GetById(id);
        if (artist == null)
            return NotFound();
        return Ok(ArtistResponse.FromEntity(artist));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateArtistRequest createArtistRequest)
    {
        var artist = await _artistService.Create(new Artist
        {
            StageName = createArtistRequest.StageName,
            Description = createArtistRequest.Description,
            RecordLabelId = createArtistRequest.RecordLabelId
        });
        return Ok(ArtistResponse.FromEntity(artist));
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, UpdateArtistRequest updateArtistRequest)
    {
        var artist = await _artistService.Update(id, new Artist
        {
            StageName = updateArtistRequest.StageName,
            Description = updateArtistRequest.Description,
            RecordLabelId = updateArtistRequest.RecordLabelId
        });

        if (artist == null)
            return NotFound();
        return Ok(ArtistResponse.FromEntity(artist));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _artistService.Delete(id);
        if (!deleted)
            return NotFound();
        return NoContent();
    }
}