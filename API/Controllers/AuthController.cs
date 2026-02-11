using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MusicCatalog.Api.DTOs.Requests;
using MusicCatalog.Api.Services;
using MusicCatalog.Common.Services;

namespace MusicCatalog.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController:ControllerBase
{
    private readonly TokenService _tokenService;
    private readonly UserService _userService;

    public AuthController(TokenService tokenService, UserService userService)
    {
        _tokenService = tokenService;
        _userService = userService;
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> CreateToken(LoginRequest loginRequest)
    {
        var user = await _userService.Authenticate(loginRequest.Username, loginRequest.Password);

        if(user == null) return Unauthorized("Invalid username or password!!");

        string token = _tokenService.CreateToken(user);

        return Ok(token);
    }
}