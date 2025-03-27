using AutoMapper;
using CustomerManager.Application.Dtos.Customers;
using CustomerManager.Domain.ReadModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace CustomerManager.Application.Mapping.CustomerMapping
{
    public class CustomerMappingProfile : Profile
    {
        public CustomerMappingProfile()
        {
            CreateMap<CustomerWithCity, CustomerDto>();

        }
    }
}
