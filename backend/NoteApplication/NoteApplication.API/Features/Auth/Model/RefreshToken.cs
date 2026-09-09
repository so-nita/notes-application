using NoteApplication.API.Repositories;

namespace NoteApplication.API.Features.Auth.Model;

[TableName("RefreshTokens")]
public class RefreshToken
{
    [KeyColumn]
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public bool IsRevoked { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
}