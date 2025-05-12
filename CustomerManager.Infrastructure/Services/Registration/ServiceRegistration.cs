using CustomerManager.Application.Interfaces.Factory;
using CustomerManager.Application.Interfaces.Repositories;
using CustomerManager.Application.Services.CustomerLogic;
using CustomerManager.Infrastructure.Factory;
using CustomerManager.Infrastructure.Repositories;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Infrastructure.Services.Registration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddInfrastructureRegistration(this IServiceCollection services)
        {
            var assm = Assembly.GetExecutingAssembly();

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICityRepository, CityRepository>();
            services.AddScoped<ICityFactory, CityFactory>();

            return services;
        }
    }
}
