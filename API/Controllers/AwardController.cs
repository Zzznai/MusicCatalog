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
    public async Task<ActionResult> GetAll()
    {
        var awards = await _awardService.GetAll();
        var response = awards.Select(AwardResponse.FromEntity);
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var award = await _awardService.GetById(id);
        if (award == null)
        {
            return NotFound();
        }
        return Ok(AwardResponse.FromEntity(award));
    }

    [HttpPost]
    public async Task<ActionResult> Create(CreateAwardRequest createAwardRequest)
    {
        var award = await _awardService.Create(new Award { Name = createAwardRequest.Name, Year = createAwardRequest.Year });
        return Ok(AwardResponse.FromEntity(award));
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> Update(int id, UpdateAwardRequest updateAwardRequest)
    {
        var award = await _awardService.Update(id, updateAwardRequest.Name, updateAwardRequest.Year);
        if (award == null)
        {
            return NotFound();
        }
        return Ok(AwardResponse.FromEntity(award));
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(int id)
    {
        var deleted = await _awardService.Delete(id);

        if (deleted == false)
        {
            return NotFound();
        }
        else
        {
            return NoContent();
        }
    }

    [HttpPut("{awardId}/artist/{artistId}")]
    public async Task<IActionResult> AddWinner(int awardId, int artistId)
    {
        var added = await _awardService.AddWinner(awardId, artistId);
        if(!added) return NotFound();

        var award = await _awardService.GetById(awardId);

        return Ok(AwardResponse.FromEntity(award));
    }

}
