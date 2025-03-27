using AutoMapper;
using CustomerManager.Application.Dtos.Customers;
using CustomerManager.Application.Interfaces.Repositories;
using CustomerManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Application.Services.CustomerLogic
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper mapper;
        public ICustomerRepository customerRepository;

        public CustomerService(IMapper mapper, ICustomerRepository customerRepository)
        {
            this.mapper = mapper;
            this.customerRepository = customerRepository;
        }

        public async Task<Customer> GetCustomer(int id)
        {
            return await customerRepository.GetByIdAsync(id);
        }
        public async Task<List<Customer>> GetAllCustomer()
        {
            return await customerRepository.GetAllAsync();
        }

        public async Task<List<CustomerDto>> GetCustomers()
        {
            var customerList = await customerRepository.GetAllCustomersAsync();
            return mapper.Map<List<CustomerDto>>(customerList);
        }
    }
}
 