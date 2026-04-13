using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;

namespace TaskBridge.Application.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _dbContext;
    private readonly IConfiguration _configuration;
    public AuthService(IApplicationDbContext dbContext, IConfiguration configuration)
    {
        _dbContext = dbContext;
        _configuration = configuration;
    }
    public async Task<UserDto> Register(RegisterDto dto)
    {
        var existingUser = await _dbContext.Users.FirstOrDefaultAsync
            (e => e.Email.ToLower() == dto.Email.ToLower());
        if (existingUser != null)
        {
            throw new Exception("User with this email already exists");
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
        var user = await _dbContext.Users.FirstOrDefaultAsync
            (e => e.Email.ToLower() == dto.Email.ToLower());
        if (user == null)
        {
            throw new Exception("Invalid email or password");
        }

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(dto.Password,user.PasswordHash);
        if (!isPasswordValid)
        {
            throw new Exception("Invalid passsword or Email");
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