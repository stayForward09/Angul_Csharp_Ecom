using StackApi.Helpers;

namespace StackApi.Middleware;

public static class ExceptionMiddlewareExtension
{
    public static IApplicationBuilder UseExceptionMiddleware(this IApplicationBuilder builder)
    {
        return builder.UseMiddleware<ExceptionMiddleware>();
    }
}