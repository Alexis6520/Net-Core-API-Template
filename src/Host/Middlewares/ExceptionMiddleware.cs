using Application.ROP;
using Host.Abstractions;

namespace Host.Middlewares;

public class ExceptionMiddleware(RequestDelegate next)
{
    private readonly RequestDelegate _next = next;

    public async Task InvokeAsync(HttpContext context, ILogger<ExceptionMiddleware> logger)
    {
        try
        {
            await _next(context);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "An unhandled exception has occurred while processing the request.");
            var response = context.Response;
            response.StatusCode = StatusCodes.Status500InternalServerError;

            var body = new Response<object>
            {
                Errors = [new Error("500", "Server error")]
            };

            await response.WriteAsJsonAsync(body);
        }
    }
}

public static class WebAppExtension
{
    extension(WebApplication app)
    {
        public WebApplication UseException()
        { 
            app.UseMiddleware<ExceptionMiddleware>();
            return app;
        }
    }
}
