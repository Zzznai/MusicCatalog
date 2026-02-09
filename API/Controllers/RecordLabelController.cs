using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.DTOs.Responses;
using MusicCatalog.Common.Services;
using MusicCatalog.Common.Entities;
using MusicCatalog.Api.DTOs.Requests;
using Microsoft.AspNetCore.Authorization;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class RecordLabelController:ControllerBase
{
    private readonly RecordLabelService _recordLabelService;

    public RecordLabelController(RecordLabelService recordLabelService)
    {
        _recordLabelService = recordLabelService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var labels = await _recordLabelService.GetAll();
        var response = labels.Select(RecordLabelResponse.FromEntity);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var label = await _recordLabelService.GetById(id);
        if (label == null)
        {
            return NotFound();
        }
        return Ok(RecordLabelResponse.FromEntity(label));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateRecordLabelRequest createRecordLabelRequest)
    {
        var label = await _recordLabelService.Create(new RecordLabel
        {
            Name = createRecordLabelRequest.Name,
            BasedIn = createRecordLabelRequest.BasedIn,
            FoundedYear = createRecordLabelRequest.FoundedYear
        });
        return Ok(RecordLabelResponse.FromEntity(label));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateRecordLabelRequest updateRecordLabelRequest)
    {
        var label = await _recordLabelService.Update(id, new RecordLabel
        {
            Name = updateRecordLabelRequest.Name,
            BasedIn = updateRecordLabelRequest.BasedIn,
            FoundedYear = updateRecordLabelRequest.FoundedYear
        });

        if (label == null)
        {
            return NotFound();
        }
        return Ok(RecordLabelResponse.FromEntity(label));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _recordLabelService.Delete(id);

        if (!deleted)
        {
            return NotFound();
        }

        return NoContent();
    }

    
}