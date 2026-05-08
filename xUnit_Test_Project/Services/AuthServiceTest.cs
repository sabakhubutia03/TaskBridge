using FluentValidation;
using FluentValidation.Results;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Moq;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Services;
using TaskBridge.Domain.Entity;
using TaskBridge.Domain.Enums;
using TaskBridge.Domain.Errors;
using TaskBridge.Infrastructure.Data;

namespace xUnit_Test_Project.Services;

public class AuthServiceTest
{
    private readonly AppDbContext _dbcontext;
    private readonly AuthService _authService;
    private readonly Mock<IValidator<RegisterDto>> _mockRegisterValidator;
    private readonly Mock<IValidator<LoginDto>> _mockLoginValidator;
    private readonly Mock<IConfiguration> _mockConfiguration;

    public AuthServiceTest()
    {
        var option = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;
        
        _dbcontext = new AppDbContext(option); 
        _mockRegisterValidator = new Mock<IValidator<RegisterDto>>();
        _mockLoginValidator = new Mock<IValidator<LoginDto>>(); 
        _mockConfiguration = new Mock<IConfiguration>();
        
        _mockConfiguration.Setup(c => c["JwtSettings:Key"])
            .Returns("this-is-a-test-secret-key-123456");

        _authService = new AuthService(
            _dbcontext,
            _mockConfiguration.Object,
            _mockRegisterValidator.Object,
            _mockLoginValidator.Object
            );
    }

    [Fact]
    public async Task Auth_WhenRegisterIsValid_ReturnsTrue()
    {
        var dto = new RegisterDto
        {
            Email = "test@gmail.com",
            Password = "password",
            FirstName = "test",
            LastName = "test"
        };

        _mockRegisterValidator
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());

        var result = await _authService.Register(dto);
        Assert.NotNull(result);
    }
    
    [Fact]
    public async Task Auth_WhenRegisterEmailFails_ReturnsFalse()
    {
        var dto = new RegisterDto
        {
            Email = "",
            Password = "password",
            FirstName = "test",
            LastName = "test"
        };
        
        _mockRegisterValidator
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new  ValidationResult( new List<ValidationFailure>
            {
                new ValidationFailure("Email", "Email is required!") 
            })); 
        
        await Assert.ThrowsAsync<ApiException>(() => 
            _authService.Register(dto));
    }

    [Fact]
    public async Task Auth_WhenRegisterPasswordFails_ReturnsFalse()
    {
        var dto = new RegisterDto
        {
            Email = "test@gmail.com",
            Password = "",
            FirstName = "test",
            LastName = "test"
        }; 
        
        _mockRegisterValidator
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("Password", "Password is required!")
            }));

        await Assert.ThrowsAsync<ApiException>(() =>
            _authService.Register(dto));
    }

    [Fact]
    public async Task Auth_WhenRegisterFirstNameFails_ReturnsFalse()
    {
        var dto = new RegisterDto
        {
            Email = "test@gmail.com",
            Password = "password",
            FirstName = "",
            LastName = "test"
        };

        _mockRegisterValidator
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("FirstName", "FirstName is required!")
            }));
        
        await Assert.ThrowsAsync<ApiException>(() => _authService.Register(dto));
    }

    [Fact]
    public async Task Auth_WhenRegisterLastNameFails_ReturnsFalse()
    {
        var dto = new RegisterDto
        {
            Email = "test@gmail.com",
            Password = "password",
            FirstName = "test",
            LastName = ""
        };

        _mockRegisterValidator
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("LastName", "LastName is required!")
            }));
        
        await Assert.ThrowsAsync<ApiException>(() => _authService.Register(dto));
    }

    [Fact]
    public async Task Auth_WhenRegisterEmailAlreadyExists_ReturnsFalse()
    {
        var exsistingUser = new User
        {
            Id = Guid.NewGuid(),
            Email = "Test@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            Role = Role.User,
            CreatedAt = DateTime.Now
        }; 
        _dbcontext.Users.Add(exsistingUser);
        await _dbcontext.SaveChangesAsync();

        var dto = new RegisterDto
        {
            Email = "Test@gmail.com",
            Password = "password",
            FirstName = "test",
            LastName = "test"
        };

        _mockRegisterValidator
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());
        
       await Assert.ThrowsAsync<ApiException>(() => _authService.Register(dto));
    } 
    
    [Fact]
    public async Task Auth_WhenLoginIsValid_ReturnsTrue()
    { 
        var register  = new RegisterDto
        {
            Email = "test@gmail.com",
            Password = "password",
            FirstName = "test",
            LastName = "test"
        };
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = register.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(register.Password),
            Role = Role.User,
            CreatedAt = DateTime.Now,
        };
        _dbcontext.Users.Add(user);
        await _dbcontext.SaveChangesAsync();

        var login = new LoginDto
        {
            Email = register.Email,
            Password = "password",
        };
        
        _mockLoginValidator
            .Setup(v => v.ValidateAsync(login, default))
            .ReturnsAsync(new ValidationResult());
        
        var result = await _authService.Login(login);
        Assert.NotNull(result);
    }

    [Fact]
    public async Task Auth_WhenLoginEmailFails_ReturnsFalse()
    {
        var dto = new LoginDto
        {
            Email = "",
            Password = "password",
        };
        
        _mockLoginValidator
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult(new List<ValidationFailure>()
            {
                new ValidationFailure("Email", "Email is required!")
            })); 
        await Assert.ThrowsAsync<ApiException>(() => _authService.Login(dto));
    }

    [Fact]
    public async Task Auth_WhenLoginPasswordFails_ReturnsFalse()
    {
        var dto = new LoginDto { Email = "test@gmail.com",  Password = ""};
        _mockLoginValidator
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult( new List<ValidationFailure>()
            {
                new ValidationFailure("Password", "Password is required!")
            })); 
        await Assert.ThrowsAsync<ApiException>(() => _authService.Login(dto));
    }

    [Fact]
    public async Task Auth_WhenLoginUserNotFound_ReturnsFalse()
    {
        var dto = new LoginDto
        {
            Email = "Test@gmail.com" ,  Password = "password"
        };
        
        _mockLoginValidator
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult());
        
        await Assert.ThrowsAsync<ApiException>(() => _authService.Login(dto));
    }

    [Fact]
    public async Task Auth_WhenLoginPasswordIsWrong_ThrowsApiException()
    {
        var user = new User
        {
            Id = Guid.NewGuid(),
            Email = "test@gmail.com",
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("password"),
            Role = Role.User,
            CreatedAt = DateTime.Now
        }; 
        _dbcontext.Users.Add(user);
        await _dbcontext.SaveChangesAsync();

        var dto = new LoginDto
        {
            Email = user.Email,
            Password = "False-Password",
        };
        
        _mockLoginValidator
            .Setup(v => v.ValidateAsync(dto, default))
            .ReturnsAsync(new ValidationResult()); 
        
        await Assert.ThrowsAsync<ApiException>(() => _authService.Login(dto));
            
    }
}