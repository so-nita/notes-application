using System.ComponentModel.DataAnnotations;

namespace NoteApplication.API.Features.Auth.Dto;

public class LoginReq
{
    [Required(ErrorMessage = "Username is required.")]
    public string Username { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required.")]
    public string Password { get; set; } = string.Empty;
}