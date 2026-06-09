using Microsoft.EntityFrameworkCore;
using Moq;
using StackExchange.Redis;
using TaskBridge.Application.Queries;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Handlers;

public class GetAllApplicationQueryHandlerTest
{
    private readonly AppDbContext _context;
    private readonly Mock<IConnectionMultiplexer> _redis;
    private readonly GetAllApplicationQueryHandler _handler;

    public GetAllApplicationQueryHandlerTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _redis = new Mock<IConnectionMultiplexer>();
        var mockRedisDatabase = new Mock<IDatabase>();

        _redis
            .Setup(x => x.GetDatabase(It.IsAny<int>(), It.IsAny<object>()))
            .Returns(mockRedisDatabase.Object);

        _handler = new GetAllApplicationQueryHandler(
            _context,
            _redis.Object);
    }

    [Fact]
    public async Task GetAllApplication_WhenISValid_RetrunTrue()
    {
        var taskItem = new TaskItem
        {
            Id = Guid.NewGuid(),
            Budget = 100,
            Description = "description",
            Title = "title",
            Status = Status.Open,
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.Now
        };
        await _context.Tasks.AddAsync(taskItem);
        await _context.SaveChangesAsync();

        var application = new Applicationn
        {
            Id = Guid.NewGuid(),
            TaskItemId = taskItem.Id,
            FreelancerId = Guid.NewGuid(),
            Status = Status.Open,
            Created = DateTime.UtcNow
        };
        await _context.Applications.AddAsync(application);
        await _context.SaveChangesAsync();
        
        var query = await _handler.Handle(new GetAllApplicationQuery(), CancellationToken.None); 
        Assert.NotEmpty(query);
    }
}