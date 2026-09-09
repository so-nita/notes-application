using System.Net;
using NoteApplication.API.Common;
using NoteApplication.API.Features.Auth.Model;
using NoteApplication.API.Features.Auth.Service;
using NoteApplication.API.Features.Notes.Model;
using NoteApplication.API.Repositories;

namespace NoteApplication.API.Features.Notes.Service;

public interface INoteService
{
    public Task<Result<List<NoteResponse>>> GetAllAsync();
    public Task<Result<NoteResponse>?> GetByIdAsync(string id);
    public Task<Result<string>> CreateAsync(NoteCreateReq request);
    public Task<Result<string>> UpdateAsync(string id, NoteUpdateReq request);
    public Task<Result<string>> DeleteAsync(string id);
}

public class NoteService : INoteService
{
    private readonly IRepository<Note> _noteRepo;
    private readonly ICurrentUserService _currentUser;

    public NoteService(IRepository<Note> noteRepository, ICurrentUserService currentUserService)
    {
        _noteRepo = noteRepository;
        _currentUser = currentUserService;
    }

    public async Task<Result<List<NoteResponse>>> GetAllAsync()
    {
        try
        {
            var notes = await _noteRepo.GetAllAsync();
            if (_currentUser.UserType != UserType.Admin.ToString())
            {
                notes = notes?.Where(e => e.CreatedBy == _currentUser.UserId).ToList();
            }

            var result = notes?.Where(e => !e.IsDeleted).Select(e => new NoteResponse()
            {
                Id = e.Id,
                Title = e.Title,
                Content = e.Content,
                CreatedAt = e.CreatedAt,
            }).ToList();

            return Result<List<NoteResponse>>.Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Result<NoteResponse>?> GetByIdAsync(string id)
    {
        try
        {
            var foundNote = await _noteRepo.GetByIdAsync(id);
            if (foundNote == null)
            {
                return Result<NoteResponse>.Fail("Note not found", HttpStatusCode.NotFound);
            }

            var result = new NoteResponse()
            {
                Id = foundNote.Id,
                Title = foundNote.Title,
                Content = foundNote.Content,
                CreatedAt = foundNote.CreatedAt,
            };

            return Result<NoteResponse>.Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<NoteResponse>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<string>> CreateAsync(NoteCreateReq request)
    {
        try
        {
            var newNote = new Note()
            {
                Id = Guid.NewGuid().ToString(),
                Title = request.Title.Trim(),
                Content = request.Content?.Trim(),
                CreatedAt = DateTime.Now,
                IsDeleted = false,
                UpdatedAt = null,
                CreatedBy = _currentUser.UserId,
            };
            
            await _noteRepo.CreateAsync(newNote);

            return Result<string>.Ok("Created Successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<string>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<string>> UpdateAsync(string id, NoteUpdateReq request)
    {
        try
        {
            var foundNote = await _noteRepo.GetByIdAsync(id);
            if (foundNote == null)
            {
                return Result<string>.Fail("Note not found", HttpStatusCode.NotFound);
            }
            
            foundNote.Title = request.Title?.Trim() ?? foundNote.Title;
            foundNote.Content = request.Content?.Trim() ?? foundNote.Content;
            
            await _noteRepo.UpdateAsync(foundNote);
            return Result<string>.Ok("Updated Successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<string>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<string>> DeleteAsync(string id)
    {
        try
        {
            var foundNote = await _noteRepo.GetByIdAsync(id);
            if (foundNote == null)
            {
                return Result<string>.Fail("Note not found", HttpStatusCode.NotFound);
            }
            
            foundNote.IsDeleted = true;
            
            await _noteRepo.UpdateAsync(foundNote);
            return Result<string>.Ok("Deleted Successfully");
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<string>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }
}