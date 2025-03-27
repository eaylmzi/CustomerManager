using CustomerManager.Domain.Entities;
using CustomerManager.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Application.Interfaces.Repositories
{
    public interface ICustomerRepository : IGenericRepository<Customer>
    {
        public Task<List<CustomerWithCity>> GetAllCustomersAsync();
    }
}
