using Microsoft.EntityFrameworkCore;
using TapFlow.API.Data;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using TapFlow.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();

var connectionString = Environment.GetEnvironmentVariable("DB_CONNECTION");
if (string.IsNullOrWhiteSpace(connectionString))
{
    connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
}


// Database configuration
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));

// Health Checks
builder.Services.AddHealthChecks()
    .AddCheck("self", () => HealthCheckResult.Healthy())
    .AddDbContextCheck<ApplicationDbContext>("database");

// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}


app.UseHttpsRedirection();

app.MapHealthChecks("/health", new HealthCheckOptions
{
    ResponseWriter = async (context, report) =>
    {
        var selfStatus = report.Entries["self"].Status;
        var dbStatus = report.Entries["database"].Status;

        var response = new
        {
            status = selfStatus == HealthStatus.Healthy ? "ok" : "fail",
            db = dbStatus == HealthStatus.Healthy ? "ok" : "fail"
        };

        context.Response.ContentType = "application/json";
        await context.Response.WriteAsJsonAsync(response);
    }
});

app.UseMiddleware<GlobalExceptionMiddleware>();

app.UseMiddleware<ApiKeyMiddleware>();

app.UseAuthorization();

app.MapControllers();

app.Run();
