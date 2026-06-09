using Microsoft.EntityFrameworkCore;
using Moq;
using TaskBridge.Application.Commands;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Handlers;

public class CreateApplicationCommandHandlerTest
{
    private readonly AppDbContext _context;
    private readonly Mock<ICurrentUserService> _currentUserService;
    private readonly CreateApplicationCommandHandler _handler;

    public CreateApplicationCommandHandlerTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);
        
        _currentUserService = new Mock<ICurrentUserService>();

        _handler = new CreateApplicationCommandHandler(
            _context,
            _currentUserService.Object
        );
    }

    [Fact]
    public async Task CreateApply_WhenValid_ShouldReturnSuccess()
    {
        var taskId = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Title",
            Description = "Description",
            Budget = 100,
            Status = Status.Open,
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.UtcNow
        };
        await _context.Tasks.AddAsync(taskId);
        await _context.SaveChangesAsync();
        
        
        var comman = new CreateApplicationCommand(taskId.Id);
        
        _currentUserService
            .Setup(x => x.UserId())
            .Returns(Guid.NewGuid());

        var result = await _handler.Handle(comman, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal(taskId.Id , result.TaskId);
    }
}