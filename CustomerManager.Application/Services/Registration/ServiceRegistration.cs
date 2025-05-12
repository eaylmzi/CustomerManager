using CustomerManager.Application.Interfaces.Repositories;
using CustomerManager.Application.Services.CustomerLogic;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Application.Services.Registration
{
    public static class ServiceRegistration
    {
        public static IServiceCollection AddApplicationRegistration(this IServiceCollection services)
        {
            var assm = Assembly.GetExecutingAssembly();

            services.AddAutoMapper(assm); 
            services.AddScoped<ICustomerService, CustomerService>();

            return services;
        }
    }
}
