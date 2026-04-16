using System.Text.Json;
using TaskBridge.Domain.Errors;

namespace TaskBridge.Middleware;

public class GlobalExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public GlobalExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    } 
    public async Task InvokeAsync(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ApiException apiException)
        {
            context.Request.ContentType = "application/json";
            var problamDetail = new ApiProblemDetails
            {
                Type = apiException.Type,
                Title = apiException.Title,
                StatusCode =  apiException.StatusCode,
                Details = apiException.Details,
                Instance = apiException.Instance
   
            };
            context.Response.StatusCode = problamDetail.StatusCode;
            var serialaz = JsonSerializer.Serialize(problamDetail ,
                new JsonSerializerOptions
                {
                    WriteIndented = true
                });
            
            await context.Response.WriteAsync(serialaz);
        }
        catch (Exception ex)
        {
            context.Response.StatusCode = 500;
            var problemDatail = new ApiProblemDetails
            {
                Type = "Internal Server Error",
                Title = "Title service error",
                StatusCode = 500,
                Details = "Internal server error",
                Instance = "Internal server  error"
            };
            context.Response.StatusCode = problemDatail.StatusCode;
    
            var serialaz = JsonSerializer.Serialize(problemDatail);
            await context.Response.WriteAsync(serialaz);
        } 
    }
}