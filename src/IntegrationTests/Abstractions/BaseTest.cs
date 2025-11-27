using Domain.Services.Persistence;
using IntegrationTests.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IntegrationTests.Abstractions;

public abstract class BaseTest(CustomWebAppFactory factory) : IClassFixture<CustomWebAppFactory>, IDisposable
{
    private IServiceScope? _scope;
    private ApplicationDbContext? _dbContext;

    protected CustomWebAppFactory Factory { get; } = factory;

    protected ApplicationDbContext DbContext
    {
        get
        {
            if (_dbContext is not null) return _dbContext;
            _scope ??= Factory.Services.CreateScope();
            _dbContext = _scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            return _dbContext;
        }
    }

    protected virtual void Cleanup() { }

    public void Dispose()
    {
        Cleanup();
        _dbContext?.Dispose();
        _scope?.Dispose();
        GC.SuppressFinalize(this);
    }
}
