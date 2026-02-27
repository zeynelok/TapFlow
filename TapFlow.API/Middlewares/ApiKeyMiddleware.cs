namespace TapFlow.API.Middlewares;

public class ApiKeyMiddleware
{
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "X-API-Key";
    private readonly string APIKEY = ""; 
    
    public ApiKeyMiddleware(RequestDelegate next, IConfiguration _configuration)
    {
        _next = next;
        APIKEY = Environment.GetEnvironmentVariable("secret-key");
    }

    public async Task InvokeAsync(HttpContext context)
    {
        if (context.Request.Path.StartsWithSegments("/health"))
        {
            await _next(context);
            return;
        }

        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }

        if (!APIKEY.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Unauthorized client.");
            return;
        }

        await _next(context);
    }
}
