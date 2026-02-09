using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.DTOs.Requests;
using MusicCatalog.Api.DTOs.Responses;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Services;

[ApiController]
[Route("api/[controller]")]
public class MoodController:ControllerBase
{
    private readonly MoodService _moodService;

   public MoodController(MoodService moodService)
    {
        _moodService = moodService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var moods = await _moodService.GetAll();
        var response = moods.Select(MoodResponse.FromEntity);
        return Ok(response);
    }

    [HttpGet("{Id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var mood = await _moodService.GetById(id);
        if (mood == null)
            return NotFound();
        return Ok(MoodResponse.FromEntity(mood));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateMoodRequest createMoodRequest)
    {
        var mood = await _moodService.Create(new Mood { Name = createMoodRequest.Name });
        return Ok(MoodResponse.FromEntity(mood));
    }


    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateMoodRequest updateMoodRequest)
    {
        var mood = await _moodService.Update(id, updateMoodRequest.Name);
        if (mood == null)
            return NotFound();
        return Ok(MoodResponse.FromEntity(mood));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete (int id)
    {

        var deleted = await _moodService.Delete(id);

        if(deleted == false)
        {
            return NotFound();
        }
        else
        {
            return NoContent();
        }

    }
}