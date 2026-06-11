using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskBridge.Application.Commands;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Handlers;

public class LoginCommandHandlerTest
{
    private readonly AppDbContext _context;
    private readonly Mock<IConfiguration> _configuration;
    private readonly LoginCommandHendler _handler;

    public LoginCommandHandlerTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        
        _configuration = new Mock<IConfiguration>();
        _configuration
            .Setup(x => x["JwtSettings:Key"])
            .Returns("this_is_my_very_long_and_super_secret_key_64_characters_long_!!!");
        
        _handler = new LoginCommandHendler(
            _context,
            _configuration.Object
        );
    }


    [Fact]
    public async Task LoginCommandHandler_LoginCommand_Success()
    {
        var register = new RegisterCommand(
            "Test@gmail.com",
            "Password123",
            "FirstNameTest",
            "LastNameTest");

        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = register.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password),
            Role = Role.User,
            CreatedAt = DateTime.Now
        };
        _context.Users.Add(user);
        await _context.SaveChangesAsync();
        
        var login = new LoginCommand(register.Email, register.Password);
        
        var reuser = await _handler.Handle(login, CancellationToken.None);
        Assert.NotNull(reuser);
    }
}