using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using NoteApplication.API.Features.Notes.Model;
using NoteApplication.API.Features.Notes.Service;

namespace NoteApplication.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class NoteController : ControllerBase
{
    private readonly INoteService _noteService;

    public NoteController(INoteService noteService)
    {
        _noteService = noteService;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var response = await _noteService.GetAllAsync();
        if (response == null || !response.IsSuccess)
        {
            return BadRequest(response);
        }
        
        return Ok(response);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        var response = await _noteService.GetByIdAsync(id);
        if (response == null || !response.IsSuccess)
        {
            return BadRequest(response);
        }
        
        return Ok(response);
    }
    
    [HttpPost]
    public async Task<IActionResult> CreateAsync([FromBody] NoteCreateReq request)
    {
        var response = await _noteService.CreateAsync(request);
        if (response == null || !response.IsSuccess)
        {
            return BadRequest(response);
        }
        
        return Ok(response);
    }
    
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateAsync(string id, [FromBody] NoteUpdateReq request)
    {
        var response = await _noteService.UpdateAsync(id, request);
        if (response == null || !response.IsSuccess)
        {
            return BadRequest(response);
        }
        
        return Ok(response);
    }
    
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(string id)
    {
        var response = await _noteService.DeleteAsync(id);
        if (response == null || !response.IsSuccess)
        {
            return BadRequest(response);
        }
        
        return Ok(response);
    }
}