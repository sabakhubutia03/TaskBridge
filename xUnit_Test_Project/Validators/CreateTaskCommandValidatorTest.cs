using Microsoft.EntityFrameworkCore;
using Moq;
using StackExchange.Redis;
using TaskBridge.Application.Commands;
using TaskBridge.Application.Interfaces;
using TaskBridge.Application.Validatorss;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Validators;

public class CreateTaskCommandValidatorTest
{
    private readonly AppDbContext _context;
    private readonly Mock<ICurrentUserService> _currentUserService;
    private readonly Mock<IConnectionMultiplexer> _redis;
    private readonly Mock<IDatabase> _mockredisDatabase;
    private readonly CreateTaskCommandHandler _handler;

    public CreateTaskCommandValidatorTest()
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
    public async Task Validator_WhenTitleEmpty_ShouldFail()
    {
        var validator = new CreateTaskCommandValidator();
        var commnad = new  CreateTaskCommand("", "Description", 10);
        
        var result = await validator.ValidateAsync(commnad);
        Assert.False(result.IsValid);
    }
}