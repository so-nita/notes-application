using Microsoft.AspNetCore.Mvc;
using NoteApplication.API.Features.Auth.Dto;
using NoteApplication.API.Features.Auth.Service;

namespace NoteApplication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IUserService _userService;

    public AuthController(IUserService userService)
    {
        _userService = userService;
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> RegisterAsync([FromBody] RegisterReq request)
    {
        var response = await _userService.RegisterAsync(request);
        if (response == null || !response.IsSuccess)
        {
            return BadRequest(response);
        }
        
        return Ok(response);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> LoginAsync([FromBody] LoginReq request)
    {
        var response = await _userService.LoginAsync(request);
        if (response == null || !response.IsSuccess)
        {
            return BadRequest(response);
        }
        
        return Ok(response);
    }
}