namespace TaskBridge.Domain.Errors;

public class ApiProblemDetails
{
    public string Type { get; init; }   
    public string Title { get; init; }
    public int StatusCode { get; init; }
    public string Details { get; init; }

    public string Instance { get; init; }
}