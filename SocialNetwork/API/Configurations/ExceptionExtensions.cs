using API.Exceptions;
using API.Middlewares;

namespace API.Configurations;

public static class ExceptionExtensions
{
    public static void AddExceptionHandling(this IServiceCollection services)
    {
        services.AddExceptionHandler<GlobalExceptionHandler>();
        services.AddProblemDetails();
    }

    public static void UseExceptionHandling(this IApplicationBuilder app)
    {
        // don't change the order of these middlewares
        app.UseExceptionHandler();
        app.UseMiddleware<UnauthorizedMiddleware>();
    }
}