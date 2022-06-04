using System.Net;

namespace StackApi.Helpers;

public class ExceptionMiddleware
{
    private readonly RequestDelegate _next;
    public ExceptionMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContent)
    {
        try
        {
            await _next(httpContent);
        }
        catch (Exception ex)
        {
            await HandleExceptionAsync(httpContent, ex);
        }
    }
    private async Task HandleExceptionAsync(HttpContext context, Exception exception)
    {
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
        await context.Response.WriteAsJsonAsync(new Response<object>()
        {
            Succeeded = false,
            Errors = new string[] { exception.Message.ToString() }
        });
    }
}