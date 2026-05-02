using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Domain.Errors;

namespace TaskBridge.Application.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    private readonly IValidator<RegisterDto> _registerDtoValidator;
    private readonly IValidator<LoginDto> _loginDtoValidator;
    public AuthService(IApplicationDbContext dbContext, IConfiguration configuration, IValidator<RegisterDto> registerDtoValidator, IValidator<LoginDto> loginDtoValidator)
    {
        _dbContext = dbContext;
        _configuration = configuration;
        _registerDtoValidator = registerDtoValidator;
        _loginDtoValidator = loginDtoValidator;
    }
    public async Task<UserDto> Register(RegisterDto dto)
    {
        var validatorResult = await _registerDtoValidator.ValidateAsync(dto);
        if (!validatorResult.IsValid)
        {
            var errorMessages = validatorResult.Errors.First().ErrorMessage;
            throw new ApiException(
                "errors/bed-request",
                "Bed Request",
                400,
                errorMessages,
                "/api/users/Register"
                );
        }
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync
            (e => e.Email.ToLower() == dto.Email.ToLower());
        if (existingUser != null)
        {
            throw new ApiException(
                "errors/conflict",
                "Conflict",
                409,
                "Email address already exists!",
                "/api/users/Register"
                );
        }

        var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = dto.Email,
            PasswordHash = passwordHash,
            Role = Role.User,
            CreatedAt = DateTime.UtcNow,
        };
        _dbContext.Users.Add(user);
        await _dbContext.SaveChangesAsync();

        return new UserDto
        {
            Id = user.Id,
            Email = user.Email,
            Role = user.Role,
            CreatedAt = user.CreatedAt,
        };
    }

    public async Task<string> Login(LoginDto dto)
    {
        var validatorResult = await _loginDtoValidator.ValidateAsync(dto);
        if (!validatorResult.IsValid)
        {
            var erroMessages = validatorResult.Errors.First().ErrorMessage;
            throw new ApiException(
                "errors/bed-request",
                "Bed Request",
                400,
                erroMessages,
                "/api/users/Login"
            );
        }
        var user = await _dbContext.Users.FirstOrDefaultAsync
            (e => e.Email.ToLower() == dto.Email.ToLower());
        if (user == null)
        {
            throw new ApiException(
                "errors/Bad Request",
                "Bad Request",
                400,
                "Invalid email or password",
                "/api/users/Login"
            );
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password,user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new ApiException(
                "errors/unauthorized",
                "Unauthorized",
                401,
                "Invalid email or password",
                "/api/users/Login"
                );
        }

        var tokeHendler = new JwtSecurityTokenHandler();
        var key = Encoding.ASCII.GetBytes(_configuration["JwtSettings:Key"]);

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(new[]
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.Role.ToString())
            }),
            Expires = DateTime.UtcNow.AddDays(2),
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature
            )
        };

        var token = tokeHendler.CreateToken(tokenDescriptor);
        return tokeHendler.WriteToken(token);

    }
}