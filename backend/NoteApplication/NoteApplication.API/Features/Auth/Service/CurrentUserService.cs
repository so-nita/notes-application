using System.Security.Claims;

namespace NoteApplication.API.Features.Auth.Service;

public interface ICurrentUserService
{
    string UserId { get; }
    string? Username { get; }
    string? UserType { get; }
}

public class CurrentUserService : ICurrentUserService
{
    private readonly IHttpContextAccessor _httpContextAcc;

    public CurrentUserService(IHttpContextAccessor httpContextAcc)
    {
        _httpContextAcc = httpContextAcc;
    }
    
    private ClaimsPrincipal? _Claims => _httpContextAcc.HttpContext?.User;

    public string UserId => _Claims?.FindFirstValue(ClaimTypes.NameIdentifier)!;
    public string? Username => _Claims?.FindFirstValue(ClaimTypes.Name);
    public string? UserType =>  _Claims?.FindFirstValue(ClaimTypes.Role);
}