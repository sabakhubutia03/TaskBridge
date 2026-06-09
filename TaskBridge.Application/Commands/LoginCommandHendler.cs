using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Errors;

namespace TaskBridge.Application.Commands;

public class LoginCommandHendler : IRequestHandler<LoginCommand , string>
{
    private readonly IApplicationDbContext _context;
    private readonly IConfiguration _configuration;

    public LoginCommandHendler(IApplicationDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }
    public async Task<string> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var user = await _context.Users.FirstOrDefaultAsync
            (e => e.Email.ToLower() == request.Email.ToLower());

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

        var isPasswordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
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

        var tokenHendler = new JwtSecurityTokenHandler();
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
        var token = tokenHendler.CreateToken(tokenDescriptor);
        return tokenHendler.WriteToken(token);
    }
}