using Dapper;
using LU2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ProjectMap.WebApi.Repositories
{
    public class EnviromentRepository  // Keeping the original name "EnviromentRepository"
    {
        private readonly string _sqlConnectionString;

        public EnviromentRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        // Read all environments
        public async Task<IEnumerable<Environment2D>> GetAllEnvironments(string id)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();

                var userId = Guid.Parse(id);

                // Retrieve raw data from the database
                var rawData = await sqlConnection.QueryAsync<dynamic>(
                    "SELECT * FROM [Environments2D] WHERE UserId = @UserId", new { UserId = userId });

                // Manually map the data to the object with null checks
                var environments = new List<Environment2D>();
                foreach (var data in rawData)
                {
                    var environment = new Environment2D
                    {
                        Id = data.Id != null ? Guid.Parse(data.Id) : Guid.Empty,
                        Name = data.Name ?? string.Empty,
                        MaxHeight = data.MaxHeight != null ? (float)data.MaxHeight : 0,
                        MaxLength = data.MaxLength != null ? (float)data.MaxLength : 0,
                        userId = data.UserId != null ? Guid.Parse(data.UserId) : Guid.Empty
                    };
                    environments.Add(environment);
                }

                return environments;
            }
        }






        // Read a specific environment by ID
        public async Task<Environment2D?> GetByIdAsync(string id)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                var environmentId = Guid.Parse(id);

                // Retrieve raw data from the database
                var rawData = await sqlConnection.QuerySingleOrDefaultAsync<dynamic>(
                    "SELECT * FROM [Environments2D] WHERE Id = @Id", new { Id = environmentId });

                if (rawData == null)
                    return null;

                // Manually map the data to the object with null checks
                var environment = new Environment2D
                {
                    Id = rawData.Id != null ? Guid.Parse(rawData.Id) : Guid.Empty,
                    Name = rawData.Name ?? string.Empty,
                    MaxHeight = rawData.MaxHeight != null ? (float)rawData.MaxHeight : 0,
                    MaxLength = rawData.MaxLength != null ? (float)rawData.MaxLength : 0,
                    userId = rawData.UserId != null ? Guid.Parse(rawData.UserId) : Guid.Empty
                };

                return environment;
            }
        }


        // Create a new environment
        public async Task<Environment2D> CreateAsync(Environment2D environment)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
               await sqlConnection.OpenAsync();
                
                environment.Id = Guid.NewGuid();
                environment.userId = Guid.Parse(environment.userId.ToString());

                var query = "INSERT INTO [Environments2D] (Id, Name, MaxHeight, MaxLength, userId) " +
                        "VALUES (@Id, @Name, @MaxHeight, @MaxLength, @userId);";


            // Execute the query
            await sqlConnection.ExecuteAsync(query, environment);

            return environment;
            }
        }

        // Update an existing environment
        public async Task UpdateAsync(Environment2D environment)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                var query = "UPDATE [Environments2D] SET Name = @Name, MaxHeight = @MaxHeight, " +
                            "MaxLength = @MaxLength, userId = @userId WHERE Id = @Id";
                await sqlConnection.ExecuteAsync(query, environment);
            }
        }

        // Delete an environment by ID
        public async Task DeleteAsync(string id)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                var query = "DELETE FROM [Environments2D] WHERE Id = @Id";
                await sqlConnection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
