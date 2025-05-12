using AutoMapper;
using CustomerManager.Application.Dtos.Customers;
using CustomerManager.Application.Interfaces.Repositories;
using CustomerManager.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CustomerManager.Application.Constants.ApiResponse;
using CustomerManager.Application.Dtos.Cities;
using CustomerManager.Application.Interfaces.Factory;
using CustomerManager.Application.Common;
using CustomerManager.Application.Constants.Table;

namespace CustomerManager.Application.Services.CustomerLogic
{
    public class CustomerService : ICustomerService
    {
        private readonly IMapper mapper;
        private readonly ICustomerRepository customerRepository;
        private readonly ICityRepository cityRepository;
        private readonly ICityFactory cityFactory;

        public CustomerService(
            IMapper mapper,
            ICustomerRepository customerRepository,
            ICityRepository cityRepository,
            ICityFactory cityFactory)
        {
            this.mapper = mapper;
            this.customerRepository = customerRepository;
            this.cityRepository = cityRepository;
            this.cityFactory = cityFactory;
        }

        public async Task<ApiResponse<int>> AddCustomer(CustomerDto customerDto)
        {
            int cityId = await GetOrCreateCityIdAsync(customerDto.CityName, customerDto.CityCountry);

            Customer customer = mapper.Map<CustomerDto, Customer>(customerDto, opt =>
            {
                opt.Items[CustomerColumns.CITY_ID] = cityId;
            });

            int id = await customerRepository.AddAsync(customer);
            if (id <= 0)
            {
                return ApiResponse<int>.CreateFailureMessage(Error.OBJECT_NOT_ADDED);
            }

            return ApiResponse<int>.CreateSuccessMessage(id, Success.OBJECT_ADDED);
        }

        public async Task<ApiResponse<bool>> DeleteCustomer(int id)
        {
            bool affectedRows = await customerRepository.DeleteAsync(id);
            return affectedRows
                ? ApiResponse<bool>.CreateSuccessMessage(affectedRows, Success.OBJECT_DELETED)
                : ApiResponse<bool>.CreateFailureMessage(Error.OBJECT_NOT_DELETED);
        }

        public async Task<ApiResponse<CustomerDto>> GetCustomer(int id)
        {
            Customer customer = await customerRepository.GetByIdAsync(id);
            if (customer is null)
            {
                return ApiResponse<CustomerDto>.CreateFailureMessage(Error.NOT_FOUND_USER);
            }

            CustomerDto customerDto = await CreateCustomerDtoAsync(customer);
            return ApiResponse<CustomerDto>.CreateSuccessMessage(customerDto, Success.OBJECT_RETRIEVED);
        }

        public async Task<ApiResponse<List<CustomerDto>>> GetCustomers()
        {
            var customerList = await customerRepository.GetAllCustomersAsync();
            if (customerList == null || customerList.Count == 0)
            {
                return ApiResponse<List<CustomerDto>>.CreateFailureMessage(Error.EMPTY_LIST);
            }

            var dtoList = mapper.Map<List<CustomerDto>>(customerList);
            return ApiResponse<List<CustomerDto>>.CreateSuccessMessage(dtoList, Success.LIST_RETRIEVED);
        }

        public async Task<ApiResponse<CustomerDto>> UpdateCustomer(int id, CustomerDto customerDto)
        {
            int cityId = await GetOrCreateCityIdAsync(customerDto.CityName, customerDto.CityCountry);

            Customer customer = mapper.Map<Customer>(customerDto, opts =>
            {
                opts.Items[CustomerColumns.CITY_ID] = cityId;
                opts.Items[CustomerColumns.ID] = id;
            });

            Customer? updatedCustomer = await customerRepository.UpdateAsync(customer);
            if (updatedCustomer == null)
            {
                return ApiResponse<CustomerDto>.CreateFailureMessage(Error.OBJECT_NOT_UPDATED);
            }

            CustomerDto updatedCustomerDto = await CreateCustomerDtoAsync(updatedCustomer);
            return ApiResponse<CustomerDto>.CreateSuccessMessage(updatedCustomerDto, Success.OBJECT_UPDATED);
        }

        private async Task<CustomerDto> CreateCustomerDtoAsync(Customer customer)
        {
            if (customer.CityId != null)
            {
                City city = await cityRepository.GetByIdAsync((int)customer.CityId);
                return mapper.Map<Customer, CustomerDto>(customer, opt =>
                {
                    opt.Items[CityColumns.CITY_NAME] = city.CityName;
                    opt.Items[CityColumns.CITY_COUNTRY] = city.CityCountry;
                });
            }

            return mapper.Map<Customer, CustomerDto>(customer, opt =>
            {
                opt.Items[CityColumns.CITY_NAME] = null;
                opt.Items[CityColumns.CITY_COUNTRY] = null;
            });
        }

        private async Task<int> GetOrCreateCityIdAsync(string cityName, string cityCountry)
        {
            int cityId = cityRepository.GetCityIdByNameAndCountry(cityName, cityCountry);
            if (cityId == -1)
            {
                cityId = await cityRepository.AddAsync(new CityDto(cityName, cityCountry));
            }
            return cityId;
        }
    }
}
