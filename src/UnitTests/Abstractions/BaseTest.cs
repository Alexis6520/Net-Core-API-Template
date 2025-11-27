using UnitTests.Services;

namespace UnitTests.Abstractions;

public abstract class BaseTest<T> : IDisposable where T : class
{
    private MemoryDbContext? _dbContext;

    public MemoryDbContext DbContext => _dbContext ??= new MemoryDbContext(typeof(T).Name);

    public void Dispose()
    {
        _dbContext?.Dispose();
        GC.SuppressFinalize(this);
    }
}
