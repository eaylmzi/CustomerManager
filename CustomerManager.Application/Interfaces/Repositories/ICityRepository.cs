using CustomerManager.Application.Dtos.Cities;
using CustomerManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Application.Interfaces.Repositories
{
    public interface ICityRepository : IGenericRepository<City>
    {
        public Task<int> AddAsync(CityDto cityDto);
        public int GetCityIdByNameAndCountry(string cityName, string cityCountry);
    }
}
