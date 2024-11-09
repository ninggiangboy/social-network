namespace API.Middlewares;

public class UnauthorizedMiddleware(RequestDelegate next)
{
    public async Task Invoke(HttpContext context)
    {
        await next(context);

        if (context.Response.StatusCode == StatusCodes.Status401Unauthorized)
        {
            throw new UnauthorizedAccessException();
        }
    }
}