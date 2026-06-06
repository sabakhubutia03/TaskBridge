using Microsoft.EntityFrameworkCore;
using TaskBridge.Application.Commands;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Domain.Errors;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Handlers;

public class DeleteTaskCommandHandlerTest
{
    private readonly AppDbContext _context;
    private readonly DeleteTaskCommandHandler _handler;

    public DeleteTaskCommandHandlerTest()
    {
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        _context = new AppDbContext(options);

        _handler = new DeleteTaskCommandHandler(_context);
    }

    [Fact]
    public async Task DeleteTask_WhenExists_ShouldReturnTaskDto()
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Budget = 100,
            Description = "Description",
            Status = Status.Open,
            Title = "Title",
            UserId = Guid.NewGuid(),
            CreatedAt = DateTime.Now
        };
        await _context.Tasks.AddAsync(task);
        await _context.SaveChangesAsync();
        
        var comman = new DeleteTaskCommand(task.Id);
        var result = await _handler.Handle(comman, CancellationToken.None);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task DeleteTask_WhenNotExists_ShouldThrowApiException()
    {
        var fackId = Guid.NewGuid();
        
        var comman = new DeleteTaskCommand(fackId);
        
        await Assert.ThrowsAsync<ApiException>(() => 
            _handler.Handle(comman, CancellationToken.None));
        
    }
}