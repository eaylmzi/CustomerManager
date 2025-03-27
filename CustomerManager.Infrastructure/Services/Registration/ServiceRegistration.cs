using CustomerManager.Application.Interfaces.Repositories;
using CustomerManager.Application.Services.CustomerLogic;
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

            //services.AddMediatR(assm);
            //services.AddAutoMapper(assm);

            //services.AddAutoMapper(assm); 
            services.AddSingleton<ICustomerRepository, CustomerRepository>();

            return services;
        }
    }
}
