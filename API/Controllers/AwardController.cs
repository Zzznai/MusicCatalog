using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.DTOs.Requests;
using MusicCatalog.Api.DTOs.Responses;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Services;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AwardController : ControllerBase
{
    private readonly AwardService _awardService;

    public AwardController(AwardService awardService)
    {
        _awardService = awardService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<ActionResult> GetAll()
    {
        var awards = await _awardService.GetAll();
        var response = awards.Select(AwardResponse.FromEntity);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var award = await _awardService.GetById(id);
        if (award == null)
        {
            return NotFound("Award not found.");
        }
        return Ok(AwardResponse.FromEntity(award));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Create(CreateAwardRequest createAwardRequest)
    {
        var award = await _awardService.Create(new Award { Name = createAwardRequest.Name, Year = createAwardRequest.Year });
        return Ok(AwardResponse.FromEntity(award));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Update(int id, UpdateAwardRequest updateAwardRequest)
    {
        var award = await _awardService.Update(id, updateAwardRequest.Name, updateAwardRequest.Year);
        if (award == null)
        {
            return NotFound("Award not found.");
        }
        return Ok(AwardResponse.FromEntity(award));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _awardService.Delete(id);

        if (deleted == false)
        {
            return NotFound("Award not found.");
        }
        else
        {
            return NoContent();
        }
    }

    [HttpPut("{awardId}/artist/{artistId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> AddWinner(int awardId, int artistId)
    {
        var award = await _awardService.GetById(awardId);
        if (award == null) return NotFound("Award not found.");
        if (AwardService.HasArtist(award, artistId))
            return BadRequest("Award already has this artist.");

        var added = await _awardService.AddWinner(awardId, artistId);
        if (!added) return NotFound("Artist not found.");

        award = await _awardService.GetById(awardId);
        return Ok(AwardResponse.FromEntity(award));
    }

    [HttpDelete("{awardId}/artist/{artistId}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> RemoveWinner(int awardId, int artistId)
    {
        var award = await _awardService.GetById(awardId);
        if (award == null) return NotFound("Award not found.");
        if (!AwardService.HasArtist(award, artistId))
            return BadRequest("Award does not have this artist.");

        var removed = await _awardService.RemoveWinner(awardId, artistId);
        if (!removed) return NotFound("Artist not found.");

        return NoContent();
    }

}
