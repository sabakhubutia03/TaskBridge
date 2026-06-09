using TaskBridge.Application.Commands;
using TaskBridge.Application.Validatorss;

namespace xUnit_Test_Project.Validators;

public class CreateApplicationCommandValidatorTest
{
    private readonly CreateApplicationCommandValidator _validator;

    public CreateApplicationCommandValidatorTest()
    {
        _validator = new CreateApplicationCommandValidator();
    }

    [Fact]
    public async Task Validaotr_WhenTaskIdEmpty_SholdFaild()
    {
        var command = new CreateApplicationCommand(Guid.Empty);
        
        var result = await _validator.ValidateAsync(command);
        Assert.False(result.IsValid);
    }
}