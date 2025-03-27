using CustomerManager.Application.Dtos.Customers;
using CustomerManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Application.Services.CustomerLogic
{
    public interface ICustomerService
    {
        public Task<Customer> GetCustomer(int id);
        public Task<List<Customer>> GetAllCustomer();
        public Task<List<CustomerDto>> GetCustomers();
    }
}
