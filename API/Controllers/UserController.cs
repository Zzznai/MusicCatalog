using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.DTOs.Requests;
using MusicCatalog.Api.DTOs.Responses;
using MusicCatalog.Common.Entities;
using MusicCatalog.Common.Services;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class UserController:ControllerBase
{
    private readonly UserService _userService;

    public UserController(UserService userService)
    {
        _userService = userService;
    }


    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var users = await _userService.GetAll();

        var response = users.Select(UserResponse.FromEntity);

        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(int id)
    {
        var user = await _userService.GetById(id);
        if(user == null)
        {
            return NotFound();
        }

        return Ok(UserResponse.FromEntity(user));
    }

    [HttpGet("username/{username}")]
    public async Task<IActionResult> GetByUsername(string username)
    {
        var user = await _userService.GetByUsername(username);
        if(user == null)
          return NotFound();
        
        return Ok(UserResponse.FromEntity(user));
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateUserRequest createUserRequest)
    {
        var user = await _userService.Create(createUserRequest.Username, createUserRequest.Password);

        return Ok(UserResponse.FromEntity(user));
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var deleted = await _userService.Delete(id);

        if(deleted == false)
          return NotFound();

        return NoContent();
    }
}