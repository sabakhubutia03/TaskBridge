using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;


namespace TaskBridge.Application.Services;

public class AuthService : IAuthService
{
    private readonly IApplicationDbContext _dbContext;
    public AuthService(IApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public Task<UserDto> Register(RegisterDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<string> Login(LoginDto dto)
    {
        throw new NotImplementedException();
    }
}