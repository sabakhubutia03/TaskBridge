using Microsoft.EntityFrameworkCore;
using Moq;
using TaskBridge.Application.Interfaces;
using TaskBridge.Application.Queries;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Domain.Errors;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Handlers;

public class GetMyTaskQueryHandlerTest
{
    private readonly AppDbContext _context;
    private readonly Mock<ICurrentUserService> _currentUserService;
    private readonly GetMyTasksQueryHandler _handler;

    public GetMyTaskQueryHandlerTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        _context = new AppDbContext(options);
        _currentUserService = new Mock<ICurrentUserService>();

        _handler = new GetMyTasksQueryHandler(
            _context,
            _currentUserService.Object);
    }

    [Fact]
    public async Task GetMyTasks_WhenTasksExsist_ShouldReturnLis()
    {
        var myTask = new TaskItem
        {
            Id = Guid.NewGuid(),
            Description = "Description",
            Title = "Title",
            Budget = 50,
            UserId = Guid.NewGuid(),
            Status = Status.Open,
            CreatedAt = DateTime.Now
        };
        _context.Tasks.Add(myTask);
        await _context.SaveChangesAsync();
        
        _currentUserService 
            .Setup(x => x.UserId())
            .Returns(myTask.UserId);
        
        var query = new GetMyTasksQuery();
        
        var result = await _handler.Handle(query, CancellationToken.None);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task GetMyTasks_WhenTasksNotExsist_ShouldReturnNull()
    {
        
        _currentUserService
            .Setup(x => x.UserId())
            .Returns(Guid.NewGuid());
        var query = new GetMyTasksQuery();
        
        await Assert.ThrowsAsync<ApiException>(() => 
            _handler.Handle(query, CancellationToken.None));
    }
}