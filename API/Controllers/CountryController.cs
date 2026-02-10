using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.DTOs.Requests;
using MusicCatalog.Api.DTOs.Responses;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Services;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CountryController : ControllerBase
{
    private readonly CountryService _countryService;

    public CountryController(CountryService countryService)
    {
        _countryService = countryService;
    }

    [HttpGet]
    [AllowAnonymous]
    public async Task<IActionResult> GetAll()
    {
        var countries = await _countryService.GetAll();
        var response = countries.Select(CountryResponse.FromEntity);
        return Ok(response);
    }

    [HttpGet("{id}")]
    [AllowAnonymous]
    public async Task<IActionResult> GetById(int id)
    {
        var country = await _countryService.GetById(id);
        if (country == null)
        {
            return NotFound();
        }
        return Ok(CountryResponse.FromEntity(country));
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Create(CreateCountryRequest createCountryRequest)
    {
        var country = await _countryService.Create(new Country { Name = createCountryRequest.Name });
        return Ok(CountryResponse.FromEntity(country));
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Update(int id, UpdateCountryRequest updateCountryRequest)
    {
        var country = await _countryService.Update(id, updateCountryRequest.Name);
        if (country == null)
        {
            return NotFound();
        }
        return Ok(CountryResponse.FromEntity(country));
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _countryService.Delete(id);

        if (deleted == false)
        {
            return NotFound();
        }
        return NoContent();
    }
}
