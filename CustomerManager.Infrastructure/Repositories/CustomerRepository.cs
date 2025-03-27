using CustomerManager.Application.Interfaces.Repositories;
using CustomerManager.Domain.Constants.Exception;
using CustomerManager.Domain.Entities;
using CustomerManager.Domain.ReadModels;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Infrastructure.Repositories
{
    public class CustomerRepository : GenericRepository<Customer>, ICustomerRepository
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public CustomerRepository()
        {
            string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException(ExceptionMessage.CONFIG_NOT_FOUND, jsonPath);

            var jsonData = File.ReadAllText(jsonPath);
            var jsonObject = JObject.Parse(jsonData);


            _connectionString = jsonObject["ConnectionStrings"]?["DatabaseConnection"]?.ToString() ?? throw new Exception(ExceptionMessage.DATABASE_NOT_FOUND);
            _tableName = typeof(Customer).Name; 
        }

        public async Task<List<CustomerWithCity>> GetAllCustomersAsync()
        {
            var customers = new List<CustomerWithCity>();


            using (var connection = new SqlConnection(_connectionString ))
            {
                await connection.OpenAsync();

                string query = @"
                    SELECT c.id, c.name, c.surname, c.balance, c.status, ct.city_name , ct.city_country 
                    FROM Customer c
                    INNER JOIN city ct ON c.city_id = ct.id";

                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
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
                }
            }

            return customers;
        }
    }
}
