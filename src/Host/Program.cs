using Application;
using Infrastructure;
using Scalar.AspNetCore;
using Serilog;
using Serilog.Events;
using Serilog.Templates;
using Serilog.Templates.Themes;

Log.Logger = new LoggerConfiguration()
    .MinimumLevel.Override("Microsoft.AspNetCore.Hosting", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Mvc", LogEventLevel.Warning)
    .MinimumLevel.Override("Microsoft.AspNetCore.Routing", LogEventLevel.Warning)
    .WriteTo.Console(new ExpressionTemplate(
        "[{@t:HH:mm:ss.ff} {@l:u3} ({Substring(SourceContext, LastIndexOf(SourceContext, '.') + 1)})] {@m} traceId:{@tr}\n{@x}",
        theme: TemplateTheme.Code))
    .CreateLogger();

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.
    builder.Services
        .AddApplicationServices()
        .AddInfrastructure()
        .AddSerilog();

    builder.Services.AddControllers()
        .ConfigureApiBehaviorOptions(opts =>
        {
            opts.SuppressModelStateInvalidFilter = true;
        });

    // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
    builder.Services.AddOpenApi();

    var app = builder.Build();
    app.UseSerilogRequestLogging();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.MapOpenApi();
        app.MapScalarApiReference();
    }

    app.UseHttpsRedirection();

    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly");
    throw;
}
finally
{
    Log.CloseAndFlush();
}