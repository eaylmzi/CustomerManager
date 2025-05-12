using CustomerManager.Application.Interfaces.Repositories;
using CustomerManager.Domain.Constants.Exception;
using Microsoft.Data.SqlClient;
using Newtonsoft.Json.Linq;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Reflection;

namespace CustomerManager.Infrastructure.Repositories
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class, new()
    {
        private readonly string _connectionString;
        private readonly string _tableName;

        public GenericRepository()
        {
            var jsonPath = Path.Combine(Directory.GetCurrentDirectory(), "appsettings.json");

            if (!File.Exists(jsonPath))
                throw new FileNotFoundException(ExceptionMessage.CONFIG_NOT_FOUND, jsonPath);

            var jsonData = File.ReadAllText(jsonPath);
            var jsonObject = JObject.Parse(jsonData);

            _connectionString = jsonObject["ConnectionStrings"]?["DatabaseConnection"]?.ToString()
                                ?? throw new Exception(ExceptionMessage.DATABASE_NOT_FOUND);

            _tableName = typeof(T).Name;
        }

        public async Task<List<T>> GetAllAsync()
        {
            var result = new List<T>();
            var query = $"SELECT * FROM {_tableName}";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            await connection.OpenAsync();

            using var reader = await command.ExecuteReaderAsync();
            while (await reader.ReadAsync())
            {
                result.Add(MapReaderToEntity(reader));
            }

            return result;
        }

        public async Task<T?> GetByIdAsync(int id)
        {
            var query = $"SELECT * FROM {_tableName} WHERE id = @id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            using var reader = await command.ExecuteReaderAsync();

            return await reader.ReadAsync() ? MapReaderToEntity(reader) : null;
        }

        public async Task<int> AddAsync(T entity)
        {
            var properties = GetNonKeyProperties();
            var columnNames = properties.Select(p => GetColumnName(p)).ToList();
            var paramNames = properties.Select(p => "@" + p.Name).ToList();

            var query = $"""
                         INSERT INTO {_tableName} ({string.Join(", ", columnNames)})
                         VALUES ({string.Join(", ", paramNames)});
                         SELECT SCOPE_IDENTITY();
                         """;

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            foreach (var prop in properties)
            {
                command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(entity) ?? DBNull.Value);
            }

            await connection.OpenAsync();
            var result = await command.ExecuteScalarAsync();
            return Convert.ToInt32(result ?? throw new Exception(ExceptionMessage.FAILED_INSERTION));
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var query = $"DELETE FROM {_tableName} WHERE id = @id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);
            command.Parameters.AddWithValue("@id", id);

            await connection.OpenAsync();
            var affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows > 0;
        }

        public async Task<T?> UpdateAsync(T entity)
        {
            var properties = typeof(T).GetProperties();
            var keyProperty = properties.FirstOrDefault(p => p.GetCustomAttribute<KeyAttribute>() != null)
                              ?? throw new InvalidOperationException(ExceptionMessage.NOT_HAVE_PROPERTY);

            var keyValue = keyProperty.GetValue(entity)
                           ?? throw new InvalidOperationException(ExceptionMessage.NULL_ID_VALUE);

            var keyColumn = GetColumnName(keyProperty);
            var updateProperties = properties.Where(p => p != keyProperty).ToList();

            var setClause = string.Join(", ", updateProperties.Select(p => $"{GetColumnName(p)} = @{p.Name}"));
            var query = $"UPDATE {_tableName} SET {setClause} WHERE {keyColumn} = @Id";

            using var connection = new SqlConnection(_connectionString);
            using var command = new SqlCommand(query, connection);

            foreach (var prop in updateProperties)
            {
                command.Parameters.AddWithValue("@" + prop.Name, prop.GetValue(entity) ?? DBNull.Value);
            }

            command.Parameters.AddWithValue("@Id", keyValue);

            await connection.OpenAsync();
            var affectedRows = await command.ExecuteNonQueryAsync();

            return affectedRows > 0 ? entity : null;
        }

        private static string GetColumnName(PropertyInfo prop)
        {
            return prop.GetCustomAttribute<ColumnAttribute>()?.Name ?? prop.Name;
        }

        private static List<PropertyInfo> GetNonKeyProperties()
        {
            return typeof(T).GetProperties()
                            .Where(p => p.GetCustomAttribute<KeyAttribute>() == null)
                            .ToList();
        }

        private static T MapReaderToEntity(SqlDataReader reader)
        {
            var entity = new T();
            var properties = typeof(T).GetProperties();

            foreach (var prop in properties)
            {
                var columnName = GetColumnName(prop);
                try
                {
                    if (!reader.IsDBNull(reader.GetOrdinal(columnName)))
                    {
                        prop.SetValue(entity, reader[columnName]);
                    }
                }
                catch (IndexOutOfRangeException)
                {
                    Console.WriteLine($"Column not found in reader: {columnName}");
                }
            }

            return entity;
        }
    }
}
