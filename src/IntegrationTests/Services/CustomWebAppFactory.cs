using Infrastructure.EFCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace IntegrationTests.Services;

public class CustomWebAppFactory : WebApplicationFactory<Program>
{
    private readonly IConfiguration _configuration;
    private static bool _databaseInitialized = false;
    private static readonly Lock _lock = new();

    public CustomWebAppFactory()
    {
        var builder = new ConfigurationBuilder()
            .AddEnvironmentVariables()
            .AddUserSecrets<CustomWebAppFactory>();

        _configuration = builder.Build();

        lock (_lock)
        {
            if (!_databaseInitialized)
            {
                InitializeDatabase();
                _databaseInitialized = true;
            }
        }
    }

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseConfiguration(_configuration);
        builder.UseEnvironment("Testing");
    }

    private void InitializeDatabase()
    {
        var dbContext = new PostgreDbContext(_configuration);
        dbContext.Database.Migrate();
    }
}
