using FluentValidation.Results;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using Moq;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Services;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Domain.Errors;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Services;

public class TaskServiceTest
{
   private readonly AppDbContext _appDbContext;
   private readonly Mock<IValidator<TaskCreateDto>> _mockCreateValidator;
   private readonly Mock<IValidator<TaskUpdateDto>> _mockUpdateValidator;
   private readonly TaskService _taskService;

   public TaskServiceTest()
   {
      var options = new DbContextOptionsBuilder<AppDbContext>()
         .UseInMemoryDatabase(Guid.NewGuid().ToString())
         .Options;

      _appDbContext = new AppDbContext(options);
      _mockCreateValidator = new Mock<IValidator<TaskCreateDto>>();
      _mockUpdateValidator = new Mock<IValidator<TaskUpdateDto>>();

      _taskService = new TaskService(
         _appDbContext,
         _mockCreateValidator.Object,
         _mockUpdateValidator.Object
      );
   }

   [Fact]
   public async Task Create_TaskWhenIsValid_ShouldReturnTrue()
   {
      var currentUserId = Guid.NewGuid();
      var dto = new TaskCreateDto
      {
         Title = "Test-Title",
         Description = "Test-Description",
         Budget = 100,
      };

      _mockCreateValidator
         .Setup(v => v.ValidateAsync(dto, default))
         .ReturnsAsync(new ValidationResult());
      
      var result = await _taskService.CreateTask(dto,currentUserId);
      Assert.NotNull(result);
      Assert.Equal(dto.Title, result.Title);
      
   }

   [Fact]
   public async Task Create_TaskWhenIsValid_ShouldReturnFalse()
   {
      var dto = new TaskCreateDto
      {
         Title = "",
         Description = "",
         Budget = 0,
      };
      var userId = Guid.NewGuid();
      
      _mockCreateValidator
         .Setup(v => v.ValidateAsync(dto, default))
         .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
         {
            new ValidationFailure("Title" ,"Title is required")
         }));
      await Assert.ThrowsAsync<ApiException>(() => 
         _taskService.CreateTask(dto,userId));
   }
   [Fact]
   public async Task Create_TaskWhenDescriptionIsInvalid_ShouldReturnFalse()
   {
      var dto = new TaskCreateDto
      {
         Title = "Test-Title",
         Description = "",
         Budget = 0,
      };
      var userId = Guid.NewGuid();
      
      _mockCreateValidator
         .Setup(v => v.ValidateAsync(dto, default))
         .ReturnsAsync(new ValidationResult(new List<ValidationFailure>
         {
            new ValidationFailure("Description", "Description is required")
         })); 
      
      await Assert.ThrowsAsync<ApiException>(() => 
         _taskService.CreateTask(dto,userId));
   }

   [Fact]
   public async Task Create_TaskWhenBudgetIsInvalid_ShouldReturnFalse()
   {
      var dto = new TaskCreateDto
      {
         Title = "Test-Title",
         Description = "Test-Description",
         Budget = 0,
      };
      var userId = Guid.NewGuid();
      
      _mockCreateValidator
         .Setup(v => v.ValidateAsync(dto, default))
         .ReturnsAsync(new ValidationResult( new List<ValidationFailure>
         {
            new ValidationFailure("Budget", "Budget must be greater than 0")
         }));
      
      await Assert.ThrowsAsync<ApiException>(() =>
         _taskService.CreateTask(dto,userId));
   }

   [Fact]
   public async Task Update_TaskWhenIsValid_ShouldReturnTrue()
   {
      var dto = new TaskUpdateDto
      {
         Title = "Test-Title",
         Description = "Test-Description",
         Budget = 100,
      };
      var taskId = Guid.NewGuid();
      var userId = Guid.NewGuid();

      var existingTask = new TaskItem
      {
         Id = taskId,
         Title = "old-Title",
         Description = "old-Description",
         Budget = 50,
         UserId = userId,
         Status = Status.Open,
         CreatedAt = DateTime.UtcNow,
      };
      
      _appDbContext.Tasks.Add(existingTask);
      await _appDbContext.SaveChangesAsync();
      
      _mockUpdateValidator 
         .Setup(v => v.ValidateAsync(dto, default))
         .ReturnsAsync(new ValidationResult()); 
      
      
      var result = await _taskService.UpdateTask(taskId, dto, userId);
      Assert.NotNull(result);
      Assert.Equal(dto.Title, result.Title);
      Assert.Equal(dto.Description, result.Description);
   }
   
}