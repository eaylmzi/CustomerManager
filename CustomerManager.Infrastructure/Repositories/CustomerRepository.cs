using CustomerManager.Application.Dtos.Customers;
using CustomerManager.Application.Interfaces.Repositories;
using CustomerManager.Domain.Constants.Exception;
using CustomerManager.Domain.Entities;
using CustomerManager.Domain.ReadModels;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using System.Data;

namespace CustomerManager.Infrastructure.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly string _connectionString;
        private readonly ICityRepository _cityRepository;

        public CustomerRepository(IConfiguration configuration, ICityRepository cityRepository)
        {
            _connectionString = configuration.GetConnectionString("DatabaseConnection")
                ?? throw new Exception(ExceptionMessage.DATABASE_NOT_FOUND);

            _cityRepository = cityRepository;
        }

        public async Task<List<CustomerWithCity>> GetAllCustomersAsync()
        {
            var customers = new List<CustomerWithCity>();

            const string query = @"
                SELECT c.id, c.name, c.surname, c.balance, c.status,
                       ct.city_name, ct.city_country
                FROM Customer c
                INNER JOIN city ct ON c.city_id = ct.id";

            await using var connection = new SqlConnection(_connectionString);
            await connection.OpenAsync();

            await using var command = new SqlCommand(query, connection);
            await using var reader = await command.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                customers.Add(new CustomerWithCity
                {
                    Id = reader.GetInt32(reader.GetOrdinal("id")),
                    Name = reader.GetString(reader.GetOrdinal("name")),
                    Surname = reader.GetString(reader.GetOrdinal("surname")),
                    Balance = reader.GetDecimal(reader.GetOrdinal("balance")),
                    Status = reader.GetBoolean(reader.GetOrdinal("status")),
                    CityName = reader.GetString(reader.GetOrdinal("city_name")),
                    CityCountry = reader.GetString(reader.GetOrdinal("city_country"))
                });
            }

            return customers;
        }
    }
}
