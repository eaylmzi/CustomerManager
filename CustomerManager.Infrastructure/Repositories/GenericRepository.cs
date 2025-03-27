using CustomerManager.Application.Interfaces.Repositories;
using CustomerManager.Domain.Constants.Exception;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace CustomerManager.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class 
    {
        private readonly string _connectionString;
        private readonly string _tableName;
    
        public GenericRepository()
        {
            string jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");
            if (!File.Exists(jsonPath))
                throw new FileNotFoundException(ExceptionMessage.CONFIG_NOT_FOUND, jsonPath);

            var jsonData = File.ReadAllText(jsonPath);
            var jsonObject = JObject.Parse(jsonData);
         

            _connectionString = jsonObject["ConnectionStrings"]?["DatabaseConnection"]?.ToString() ?? throw new Exception(ExceptionMessage.DATABASE_NOT_FOUND);
            _tableName = typeof(T).Name; 
        }

        public async Task<List<T>> GetAllAsync()
        {
            var result = new List<T>();

            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                // I have to use stored procedure to prevent sql injection
                string query = $"SELECT * FROM {_tableName}";
                using (var command = new SqlCommand(query, connection))
                using (var reader = await command.ExecuteReaderAsync())
                {
                    while (await reader.ReadAsync())
                    {
                        result.Add(MapReaderToEntity(reader));
                    }
                }
            }
            return result;
        }

        public async Task<T> GetByIdAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = $"SELECT * FROM {_tableName} WHERE id = @id";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    using (var reader = await command.ExecuteReaderAsync())
                    {
                        if (await reader.ReadAsync())
                        {
                            return MapReaderToEntity(reader);
                        }
                    }
                }
            }
            return null;
        }


        public async Task<T> AddAsync(T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();

                var properties = typeof(T).GetProperties();
                var columnNames = string.Join(",", properties.Select(p => p.Name));
                var paramNames = string.Join(",", properties.Select(p => "@" + p.Name));

                string query = $"INSERT INTO {_tableName} ({columnNames}) VALUES ({paramNames}); SELECT SCOPE_IDENTITY();";

                using (var command = new SqlCommand(query, connection))
                {
                    foreach (var prop in properties)
                    {
                        command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(entity) ?? DBNull.Value);
                    }
                    await command.ExecuteNonQueryAsync();
                }
            }
            return entity;
        }
        public async Task<bool> DeleteAsync(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                string query = $"DELETE FROM {_tableName} WHERE id = @id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    int affectedRows = await command.ExecuteNonQueryAsync();
                    return affectedRows > 0;
                }
            }
        }
        public async Task<T> UpdateAsync(string id, T entity)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                await connection.OpenAsync();
                var properties = typeof(T).GetProperties();
                var setClause = string.Join(",", properties.Select(p => $"{p.Name} = @{p.Name}"));

                string query = $"UPDATE {_tableName} SET {setClause} WHERE Id = @id";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@id", id);
                    foreach (var prop in properties)
                    {
                        command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(entity) ?? DBNull.Value);
                    }
                    await command.ExecuteNonQueryAsync();
                }
            }
            return entity;
        }


        private T MapReaderToEntity(SqlDataReader reader)
        {
            var entity = Activator.CreateInstance<T>();

            foreach (var prop in typeof(T).GetProperties())
            {
                // Önce [Column] niteliğini kontrol et
                var columnAttr = prop.GetCustomAttribute<ColumnAttribute>();
                string columnName = columnAttr != null ? columnAttr.Name : prop.Name;

                try
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                    {
                        prop.SetValue(entity, reader[columnName]);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine($"Sütun bulunamadı: {columnName}");
                }
            }

            return entity;
        }

    }
}
