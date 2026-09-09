using System.ComponentModel.DataAnnotations;

namespace NoteApplication.API.Features.Auth.Dto;

public class RegisterReq
{
    public string FullName { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Username is required.")]
    [MaxLength(50, ErrorMessage = "Username must be 50 characters or less")]
    public string Username { get; set; } = string.Empty;
    
    [Required(ErrorMessage = "Password is required.")]
    [MinLength(6, ErrorMessage = "Password must be at least 6 characters or less")]
    public string Password { get; set; } = string.Empty;
}