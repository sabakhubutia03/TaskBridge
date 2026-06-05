using Microsoft.EntityFrameworkCore;
using Moq;
using StackExchange.Redis;
using TaskBridge.Application.Commands;
using TaskBridge.Application.Interfaces;
using TaskBridge.Application.Validatorss;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Handlers;

public class CreateTaskCommandHandlerTest
{
    private readonly AppDbContext _context;
    private readonly Mock<ICurrentUserService> _currentUserService;
    private readonly Mock<IConnectionMultiplexer> _redis;
    private readonly Mock<IDatabase> _mockredisDatabase;
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandHandlerTest()
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

        _handler = new CreateTaskCommandHandler(
            _context,
            _currentUserService.Object,
            _redis.Object
        );
    }

    [Fact]
    public async Task CreateTask_WhenValid_ShouldReturnTaskDto()
    {
       var comman = new CreateTaskCommand("TItle" , "Description", 10);

       _currentUserService
           .Setup(x => x.UserId())
           .Returns(Guid.NewGuid());
       
       var result = await _handler.Handle(comman , CancellationToken.None);
       Assert.NotNull(result);
       Assert.Equal("TItle", result.Title);
    }
    
}