namespace NoteApplication.API.Features.Auth.Dto;

public class AuthResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public DateTime AccessTokenExpiresAt  { get; set; }
    public string Id { get; set; } = string.Empty;
    public string Username { get; set; } = string.Empty;
}