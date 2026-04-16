namespace TaskBridge.Domain.Errors;

public class ApiException : Exception
{
    public string Type { get; init; }
    public string Title { get; init; }
    public int StatusCode { get; init; }
    public string Details { get; init; }
    public string Instance { get; init; }
    public ApiException(string type, string title, int statusCode, string details, string instance)
    {
        Type = type;
        Title = title;
        StatusCode = statusCode;
        Details = details;
        Instance = instance;

    } 
}