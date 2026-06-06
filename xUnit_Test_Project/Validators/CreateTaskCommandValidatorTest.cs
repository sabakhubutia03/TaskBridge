using TaskBridge.Application.Commands;
using TaskBridge.Application.Validatorss;


namespace xUnit_Test_Project.Validators;

public class CreateTaskCommandValidatorTest
{
   private readonly CreateTaskCommandValidator _validator;

    public CreateTaskCommandValidatorTest()
    {
        _validator = new CreateTaskCommandValidator();
    }
    
    [Fact]
    public async Task Validator_WhenTitleEmpty_ShouldFail()
    {
        var command = new CreateTaskCommand("", "Description", 10);
    
        var result = await _validator.ValidateAsync(command);
    
        Assert.False(result.IsValid);
    }
}