using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace Application;

public static class Startup
{
    extension(IServiceCollection services)
    {
        public IServiceCollection AddApplicationServices()
        {
            var assembly = typeof(Startup).Assembly;
            services.AddMediatR(x => x.RegisterServicesFromAssembly(assembly));
            services.AddValidatorsFromAssembly(assembly);
            return services;
        }
    }
}
