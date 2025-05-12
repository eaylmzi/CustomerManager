using CustomerManager.Application.Interfaces.Factory;
using CustomerManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Infrastructure.Factory
{
    public class CityFactory : ICityFactory

    {
        public object CreateCity(string cityName, string cityCountry)
        {
            return new City(cityName, cityCountry); 
        }
    }
}
