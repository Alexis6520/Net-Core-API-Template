using Domain.Services.Persistence;
using Infrastructure.EFCore;
using Microsoft.Extensions.DependencyInjection;

namespace Infrastructure;

public static class Startup
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddInfrastructure()
        {
            services.AddDbContext<ApplicationDbContext, PostgreDbContext>();
            return services;
        }
    }
}
