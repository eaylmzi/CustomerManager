using CustomerManager.Application.Common;
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
        public Task<ApiResponse<int>> AddCustomer(CustomerDto customerDto);
        public Task<ApiResponse<bool>> DeleteCustomer(int id);
        public Task<ApiResponse<CustomerDto>> GetCustomer(int id);
        public Task<ApiResponse<List<CustomerDto>>> GetCustomers();    
        public Task<ApiResponse<CustomerDto>> UpdateCustomer(int id, CustomerDto customerDto);
    }
}
