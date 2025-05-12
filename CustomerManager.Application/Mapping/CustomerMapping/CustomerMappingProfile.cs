using AutoMapper;
using CustomerManager.Application.Constants.Table;
using CustomerManager.Application.Dtos.Customers;
using CustomerManager.Domain.Entities;
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

            CreateMap<CustomerDto, Customer>()
                .ConstructUsing((dto, ctx) =>
                 new Customer(dto.Name, dto.Surname, dto.Balance, dto.Status, ctx.Items[CustomerColumns.CITY_ID] as int? ?? 0));

            CreateMap<CustomerDto, Customer>()
                .ForMember(dest => dest.CityId, opt => opt.MapFrom((src, dest, destMember, context) =>
                     context.Items.ContainsKey(CustomerColumns.CITY_ID) ? (int)context.Items[CustomerColumns.CITY_ID] : 0))
                .ForMember(dest => dest.Id, opt => opt.MapFrom((src, dest, destMember, context) =>
                        context.Items.ContainsKey(CustomerColumns.ID) ? (int)context.Items[CustomerColumns.ID] : 0));

            CreateMap<Customer, CustomerDto>()
                .ForMember(dest => dest.CityName, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items.ContainsKey(CityColumns.CITY_NAME) ? context.Items[CityColumns.CITY_NAME]?.ToString() : null))
                .ForMember(dest => dest.CityCountry, opt => opt.MapFrom((src, dest, destMember, context) =>
                    context.Items.ContainsKey(CityColumns.CITY_COUNTRY) ? context.Items[CityColumns.CITY_COUNTRY]?.ToString() : null));
        }
    }
}
