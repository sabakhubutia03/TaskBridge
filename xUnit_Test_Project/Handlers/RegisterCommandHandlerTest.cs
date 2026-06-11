using Microsoft.EntityFrameworkCore;
using TaskBridge.Application.Commands;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Handlers;

public class RegisterCommandHandlerTest
{
    private readonly AppDbContext _context;
    private readonly RegisterCommandHandler _handler;

    public RegisterCommandHandlerTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        
        _handler = new RegisterCommandHandler(
            _context
        );
    }

    [Fact]
    public async Task Register_WhenRegisterIsValid_ReturnsTrue()
    {
        var register = new RegisterCommand(
            "Test@gmail.com",
            "Password123",
            "NameTest",
            "LastNameTest");
        ;
        
        var result = await _handler.Handle(register, CancellationToken.None);
        Assert.NotNull(result);
    }
}