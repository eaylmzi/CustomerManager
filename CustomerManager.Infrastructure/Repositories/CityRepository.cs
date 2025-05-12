using CustomerManager.Application.Constants.Exception;
using CustomerManager.Application.Dtos.Cities;
using CustomerManager.Application.Interfaces.Repositories;
using CustomerManager.Domain.Entities;
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
    public class CityRepository : GenericRepository<City> , ICityRepository
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public CityRepository()
        {
            string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException(ExceptionMessage.CONFIG_NOT_FOUND, jsonPath);

            var jsonData = File.ReadAllText(jsonPath);
            var jsonObject = JObject.Parse(jsonData);


            _connectionString = jsonObject["ConnectionStrings"]?["DatabaseConnection"]?.ToString() ?? throw new Exception(ExceptionMessage.DATABASE_NOT_FOUND);
            _tableName = typeof(City).Name;
        }

        public int GetCityIdByNameAndCountry(string cityName, string cityCountry)
        {

            string query = "SELECT id FROM city WHERE city_name = @CityName AND city_country = @CityCountry";

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
            
                    connection.Open();

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {

                        command.Parameters.Add(new SqlParameter("@CityName", SqlDbType.NVarChar, 255)).Value = cityName;
                        command.Parameters.Add(new SqlParameter("@CityCountry", SqlDbType.NVarChar, 255)).Value = cityCountry;


                        var result = command.ExecuteScalar();

                        if (result != null)
                        {
                            return Convert.ToInt32(result); 
                        }
                        else
                        {
                            return -1;  
                        }
                    }

            }

        }
        public async Task<int> AddAsync(CityDto cityDto)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                string query = @"INSERT INTO city (city_name, city_country) 
                 VALUES (@Name, @Country); 
                 SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Name", cityDto.CityName);
                    command.Parameters.AddWithValue("@Country", cityDto.CityCountry);

                    object? result = await command.ExecuteScalarAsync();

                    if (result == null || result == DBNull.Value)
                        throw new Exception("Insert failed, no ID returned.");

                    return Convert.ToInt32(result);
                }
            }
            
        }


    }
}
