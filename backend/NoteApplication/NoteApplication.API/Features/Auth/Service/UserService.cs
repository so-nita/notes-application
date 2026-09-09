using System.Net;
using Microsoft.Extensions.Options;
using NoteApplication.API.Common;
using NoteApplication.API.Features.Auth.Dto;
using NoteApplication.API.Features.Auth.Model;
using NoteApplication.API.Repositories;

namespace NoteApplication.API.Features.Auth.Service;

public interface IUserService
{
    public Task<Result<AuthResponse>> RegisterAsync(RegisterReq request);
    public Task<Result<AuthResponse>> LoginAsync(LoginReq request);
}

public class UserService : IUserService
{
    private readonly IRepository<User> _userRepo;
    private readonly IRepository<RefreshToken> _refreshTokenRepo;
    private readonly ITokenService _tokenService;
    private readonly JwtSettings _jwtSettings;

    public UserService(IRepository<User> userRepository, IRepository<RefreshToken> refreshTokenRepository, ITokenService tokenService, IOptions<JwtSettings> jwtSettings)
    {
        _userRepo = userRepository;
        _refreshTokenRepo = refreshTokenRepository;
        _tokenService = tokenService;
        _jwtSettings = jwtSettings.Value;
    }
    

    public async Task<Result<AuthResponse>> RegisterAsync(RegisterReq request)
    {
        try
        {
            var username = request.Username.Trim();
            var existingUser = await _userRepo.FindOneAsync("Username = @Username", new { Username = username });
            if (existingUser != null)
            {
                return Result<AuthResponse>.Fail("Username already exists", HttpStatusCode.Conflict);
            }

            var newUser = new User()
            {
                Id = Guid.NewGuid().ToString(),
                Username = request.Username,
                Password = HashPassword(request.Password.Trim()),
                CreatedAt = DateTime.Now,
                CreatedBy = "SELF_REGISTER",
                IsDeleted = false,
                UpdatedAt = null,
                UserType = UserType.User
                // UserType = UserType.Admin
            };
            await _userRepo.CreateAsync(newUser);
            var result = await GenerateAccessToken(newUser);
            
            return Result<AuthResponse>.Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<AuthResponse>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }

    public async Task<Result<AuthResponse>> LoginAsync(LoginReq request)
    {
        try
        {
            var username = request.Username.ToUpper().Trim();
            var foundUser = await _userRepo.FindOneAsync("Username = @Username", new { Username = username });
            if (foundUser == null)
            {
                return Result<AuthResponse>.Fail("Username does not exist", HttpStatusCode.NotFound);
            }
            
            var correctPassword = VerifyPassword(request.Password, foundUser.Password);
            if (!correctPassword)
            {
                return Result<AuthResponse>.Fail("Wrong password", HttpStatusCode.Unauthorized);
            }
            
            var result = await GenerateAccessToken(foundUser);
            return Result<AuthResponse>.Ok(result);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Result<AuthResponse>.Fail(e.Message, HttpStatusCode.InternalServerError);
        }
    }
    
    private string HashPassword(string password) => BCrypt.Net.BCrypt.HashPassword(password);

    private bool VerifyPassword(string password, string passwordHash)
    {
        return BCrypt.Net.BCrypt.Verify(password, passwordHash);
    }

    private async Task<AuthResponse> GenerateAccessToken(User user)
    {
        var (token, expiresAt) = _tokenService.GenerateAccessToken(user);
        return new AuthResponse()
        {
            AccessToken = token,
            Id = user.Id,
            Username = user.Username,
            AccessTokenExpiresAt = expiresAt,
        };
    }
}