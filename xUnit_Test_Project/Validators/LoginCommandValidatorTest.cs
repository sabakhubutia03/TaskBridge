using TaskBridge.Application.Commands;
using TaskBridge.Application.Validatorss;

namespace xUnit_Test_Project.Validators;

public class LoginCommandValidatorTest
{
    private readonly LoginCommandValidator _validator;

    public LoginCommandValidatorTest()
    {
        _validator = new LoginCommandValidator();
    }

    [Fact]
    public async Task Login_WhenEmailInvalid_ShouldFail()
    {
        var login = new LoginCommand("notanemail", "password123");
        
       var result = await _validator.ValidateAsync(login);
       Assert.False(result.IsValid);
       Assert.Contains(result.Errors, e => e.PropertyName == "Email");
    }

    [Fact]
    public async Task Login_WhenPasswordInvalid_ShouldFail()
    {
        var login = new LoginCommand("test@gmail.com", "123");
        
        var result = await _validator.ValidateAsync(login);
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, p => p.PropertyName == "Password");
        
    }

    [Fact]
    public async Task Login_WhenValid_ShouldPass()
    {
        var login = new LoginCommand("test@gmail.com", "Password123");
        var result = await _validator.ValidateAsync(login);
        Assert.True(result.IsValid);
    }
}