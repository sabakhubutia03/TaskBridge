using TaskBridge.Application.Commands;
using TaskBridge.Application.Validatorss;

namespace xUnit_Test_Project.Validators;

public class RegisterCommandValidatorTest
{
    private readonly RegisterCommandValidator _validator;

    public RegisterCommandValidatorTest()
    {
        _validator = new RegisterCommandValidator();
    }

    [Fact]
    public async Task Register_WhenEmailNotValid_ReturnsFalse()
    {
        var register = new RegisterCommand(
            "Email",
            "Password123",
            "FirsName",
            "LastName");
        
        var result = await _validator.ValidateAsync(register);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors , e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Register_WhenPasswordNotValid_ReturnsFalse()
    {
        var register = new RegisterCommand("email@gmail.com", "123", "FirsName", "LastName");
        
        var result = await _validator.ValidateAsync(register);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors , e => e.PropertyName == "Password");
    }

    [Fact]
    public async Task Register_WhenFirstNameNotValid_ReturnsFalse()
    {
        var register = new RegisterCommand("email@gmail.com", "Password123", "", "LastName");
        var result = await _validator.ValidateAsync(register);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors , e => e.PropertyName == "FirstName");
    }

    [Fact]
    public async Task Register_WhenLastNameNotValid_ReturnsFalse()
    {
        var register = new RegisterCommand("email@gmail.com", "Password123", "FirsName", "");
        var result = await _validator.ValidateAsync(register);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors , e => e.PropertyName == "LastName");
    }
    
}