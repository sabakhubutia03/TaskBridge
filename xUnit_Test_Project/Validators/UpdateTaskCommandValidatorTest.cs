using TaskBridge.Application.Commands;
using TaskBridge.Application.Validatorss;


namespace xUnit_Test_Project.Validators;

public class UpdateTaskCommandValidatorTest
{
    private readonly UpdateTaskCommandValidator _validator;

    public UpdateTaskCommandValidatorTest()
    {
        _validator = new UpdateTaskCommandValidator();
    }

    [Fact]
    public async Task UpdateTask_WhenPriorityIsNegative_ShouldFail()
    {
        var command = new UpdateTaskCommand( Guid.NewGuid() , "Title", "Description" , -10);
        
        var result = await _validator.ValidateAsync(command);
        Assert.False(result.IsValid);
    }
}