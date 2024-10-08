using Credit.Infrastructure.Persistence;
using Credit.Infrastructure.Repositories;
using Credit.Infrastructure.Repositories.Credit;
using Credit.Infrastructure.Repositories.Person;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Credit.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services,
            IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");

            services
                .AddServices()
                .AddPersistence(connectionString);
            
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, string connectionString)
        {
            services.AddDbContext<MainDbContext>(options => options.UseSqlServer(connectionString));
            return services;
        }

        private static IServiceCollection AddServices(this IServiceCollection services)
        {
            //unit of work
            services.AddScoped<IUnitOfWork, UnitOfWork<MainDbContext>>();

            //repositories
            services.AddScoped<IPersonRepository, PersonRepository>();
            services.AddScoped<ICreditRepository, CreditRepository>();

            return services;
        }
    }
}
