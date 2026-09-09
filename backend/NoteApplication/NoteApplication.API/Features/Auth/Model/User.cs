using Microsoft.AspNetCore.Identity;
using NoteApplication.API.Contracts;
using NoteApplication.API.Features.Notes.Model;
using NoteApplication.API.Repositories;

namespace NoteApplication.API.Features.Auth.Model;

[TableName("Users")]
public class User : IBaseEntity<string>
{
    [KeyColumn]
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public string UserType { get; set; } = string.Empty;
    
    public string? CreatedBy { get; set; }
    public bool IsDeleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class UserType
{
    public const string Admin = "Admin";
    public const string User = "User";
}