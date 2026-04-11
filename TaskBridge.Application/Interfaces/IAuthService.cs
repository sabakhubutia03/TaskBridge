using TaskBridge.Application.DTOs;

namespace TaskBridge.Application.Interfaces;

public interface IAuthService
{
    Task<UserDto> Register(RegisterDto dto);
    Task<string> Login(LoginDto dto);
}