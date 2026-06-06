using Microsoft.EntityFrameworkCore;
using Moq;
using StackExchange.Redis;
using TaskBridge.Application.Queries;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Handlers;

public class GetAllTaskQueryHandlerTest
{
    private readonly AppDbContext _context;
    private readonly Mock<IConnectionMultiplexer> _redis;
    private readonly Mock<IDatabase> _mockredisDatabase;
    private readonly GetAllTaksQueryHandler _handler;

    public GetAllTaskQueryHandlerTest()
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

        _handler = new GetAllTaksQueryHandler(
            _context,
            _redis.Object);
    }

    [Fact]
    public async Task GetAllTasks_WhenTasksExist_ShouldReturnList()
    {
        var items = new TaskItem
        {
            Id = Guid.NewGuid(),
            Budget = 100,
            Description = "description",
            Title = "title",
            Status = Status.Open,
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.Now
        };
        await _context.Tasks.AddRangeAsync(items);
        await _context.SaveChangesAsync();

        var query = new GetAllTasksQuery();
        
        var result = await _handler.Handle(query , CancellationToken.None);
        Assert.NotNull(result);
    }
}