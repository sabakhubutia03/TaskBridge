using MassTransit;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;
using TaskBridge.Application.Messages;
using TaskBridge.Application.Services;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Domain.Errors;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Services;

public class ApplicationServiceTest
{
    private readonly AppDbContext _dbContext;
    private readonly Mock<ICurrentUserService> _currentUserServiceMock; 
    private readonly ApplicationService _applicationService;
    private readonly Mock<IPublishEndpoint> _publishEndpointMock;

    public ApplicationServiceTest()
    {
        var option = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _dbContext = new AppDbContext(option);
        _currentUserServiceMock = new Mock<ICurrentUserService>(); 
        _publishEndpointMock = new Mock<IPublishEndpoint>();
        
        _publishEndpointMock
            .Setup(x => x.Publish(It.IsAny<ApplicationSubmittedMessage>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);
        
        _applicationService = new ApplicationService(
            _dbContext,
            _currentUserServiceMock.Object ,
            _publishEndpointMock.Object
            );
        
    }

    [Fact]
    public async Task Application_WhenTaskIdIsEmpty_ThrowsApiException()
    {
        var dto = new ApplicationCreateDto
        {
            TaskId = Guid.Empty,
        };

        await Assert.ThrowsAsync<ApiException>(() =>
            _applicationService.ApplyToTask(dto));
    }

    [Fact]
    public async Task Application_WhenTaskIdIsNotFound_ThrowsApiException()
    {
        var dto = new ApplicationCreateDto
        {
            TaskId = Guid.NewGuid()
        }; 
        await Assert.ThrowsAsync<ApiException>(() =>
            _applicationService.ApplyToTask(dto)); 
    }

    [Fact]
    public async Task Application_WhenTaskIdIsValid_ReturnTrue()
    {
        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = "Test",
            Description = "Test",
            Budget = 100,
            CreatedAt = DateTime.UtcNow,
            Status = Status.Open,
            UserId = Guid.NewGuid(),
        };
        _dbContext.Tasks.Add(task);
        await _dbContext.SaveChangesAsync();

        _currentUserServiceMock
            .Setup(s => s.UserId())
            .Returns(Guid.NewGuid()); 
        
        var dto = new ApplicationCreateDto
        {
            TaskId = task.Id,
        };

        var result = await _applicationService.ApplyToTask(dto);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Application_WhenGetTaskApplications_ReturnTrue()
    {
        var application = new Applicationn
        {
          Id = Guid.NewGuid(),
          TaskItemId = Guid.NewGuid(),
          Status = Status.Open,
          FreelancerId = Guid.NewGuid(),
          Created = DateTime.UtcNow
        };
        _dbContext.Add(application);
        await _dbContext.SaveChangesAsync();

        var result = await _applicationService.GetTaskApplications();
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Application_WhenGetMyTaskApplications_ReturnTrue()
    {
        var userId = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@gmail.com",
            PasswordHash = "1sdap123213",
            Role = Role.User,
            CreatedAt = DateTime.UtcNow,
        };
        
        _dbContext.Add(userId);
        await _dbContext.SaveChangesAsync();

        var app = new Applicationn
        {
            Id = Guid.NewGuid(),
            TaskItemId = Guid.NewGuid(),
            FreelancerId = userId.Id,
            Created = DateTime.UtcNow,
            Status = Status.Open
        }; 
        _dbContext.Add(app);
        await _dbContext.SaveChangesAsync();
        
        _currentUserServiceMock
            .Setup(s => s.UserId())
            .Returns(userId.Id);

        var result = await _applicationService.GetMyApplication();
        Assert.NotNull(result);
    }
}