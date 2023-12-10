using System.Text.Json;

namespace CODL.Exceptions;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ServiceException ex)
        {
            // Handle the exception and generate a response
            await HandleServiceExceptionAsync(context, ex);
        }
    }
    private static Task HandleServiceExceptionAsync(HttpContext context, ServiceException exception)
    {
        // Generate an error response based on the exception
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int) exception.StatusCode;
        return context.Response.WriteAsync(exception.Response.ToString());
    }
}