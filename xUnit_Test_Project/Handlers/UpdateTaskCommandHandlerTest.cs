using Microsoft.EntityFrameworkCore;
using Moq;
using StackExchange.Redis;
using TaskBridge.Application.Commands;
using TaskBridge.Application.Interfaces;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Handlers;

public class UpdateTaskCommandHandlerTest
{
    private readonly AppDbContext _context;
    private readonly Mock<ICurrentUserService> _currentUserService;
    private readonly Mock<IConnectionMultiplexer> _redis;
    private readonly Mock<IDatabase> _mockredisDatabase;
    private readonly UpdateTasksCommandHandler _handler;

    public UpdateTaskCommandHandlerTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        _currentUserService = new Mock<ICurrentUserService>();
        _redis = new Mock<IConnectionMultiplexer>();
        _mockredisDatabase = new Mock<IDatabase>();

        _redis
            .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(_mockredisDatabase.Object);

        _handler = new UpdateTasksCommandHandler(
            _context,
            _currentUserService.Object,
            _redis.Object
        );
    }

    [Fact]
    public async Task UpdateTask_WhenValid_ShouldReturnTrue()
    {
        var userId = Guid.NewGuid();
        
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Old-Title",
            Description = "Old-Description",
            Budget = 50,
            UserId = userId,
            Status = Status.Open,
            CreatedAt = DateTime.Now
        };
        
        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();
        
        _currentUserService
            .Setup(x=> x.UserId())
            .Returns(userId);
        
        var command = new UpdateTaskCommand(task.Id , "New-Title", "New-Description", 100);
        var result = await _handler.Handle(command, CancellationToken.None);
        Assert.NotNull(result);
        Assert.Equal("New-Title", result.Title);
    }
}