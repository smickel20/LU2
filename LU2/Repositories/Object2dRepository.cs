using Dapper;
using LU2.Models;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace ProjectMap.WebApi.Repositories
{
    public class Object2dRepository
    {
        private readonly string _sqlConnectionString;

        public Object2dRepository(string sqlConnectionString)
        {
            _sqlConnectionString = sqlConnectionString;
        }

        // Read all objects
        public async Task<IEnumerable<Object2d>> ReadAsync()
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                return await sqlConnection.QueryAsync<Object2d>("SELECT * FROM [Object2D]");
            }
        }

        // Read a specific object by ID
        public async Task<Object2d?> GetByIdAsync(string id)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();

                var objectId = Guid.Parse(id);

                // Retrieve raw data from the database
                var rawData = await sqlConnection.QuerySingleOrDefaultAsync<dynamic>(
                    "SELECT * FROM [Object2D] WHERE Id = @Id", new { Id = objectId });

                if (rawData == null)
                    return null;

                // Manually map the data to the object with null checks
                var object2D = new Object2d
                {
                    Id = rawData.Id != null ? Guid.Parse(rawData.Id) : Guid.Empty,
                    PrefabId = rawData.PrefabId != null ? (int)rawData.PrefabId : 0,
                    PositionX = rawData.PositionX != null ? (float)rawData.PositionX : 0f,
                    PositionY = rawData.PositionY != null ? (float)rawData.PositionY : 0f,
                    ScaleX = rawData.ScaleX != null ? (float)rawData.ScaleX : 0f,
                    ScaleY = rawData.ScaleY != null ? (float)rawData.ScaleY : 0f,
                    RotationZ = rawData.RotationZ != null ? (float)rawData.RotationZ : 0f,
                    SortingLayer = rawData.SortingLayer != null ? (int)rawData.SortingLayer : 0,
                    EnvironmentId = rawData.EnvironmentId != null ? Guid.Parse(rawData.EnvironmentId) : Guid.Empty
                };

                return object2D;
            }
        }

        public async Task<IEnumerable<Object2d>> GetByEnvironmentAsync(string id)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();

                var environmentId = Guid.Parse(id);

                // Retrieve raw data from the database
                var rawData = await sqlConnection.QueryAsync<dynamic>(
                    "SELECT * FROM [Object2D] WHERE EnvironmentId = @EnvironmentId", new { EnvironmentId = environmentId });

                // Manually map the data to the object with null checks
                var objects = new List<Object2d>();
                foreach (var data in rawData)
                {
                    var object2D = new Object2d
                    {
                        Id = data.Id != null ? Guid.Parse(data.Id) : Guid.Empty,
                        PrefabId = data.PrefabId != null ? (int)data.PrefabId : 0,
                        PositionX = data.PositionX != null ? (float)data.PositionX : 0f,
                        PositionY = data.PositionY != null ? (float)data.PositionY : 0f,
                        ScaleX = data.ScaleX != null ? (float)data.ScaleX : 0f,
                        ScaleY = data.ScaleY != null ? (float)data.ScaleY : 0f,
                        RotationZ = data.RotationZ != null ? (float)data.RotationZ : 0f,
                        SortingLayer = data.SortingLayer != null ? (int)data.SortingLayer : 0,
                        EnvironmentId = data.EnvironmentId != null ? Guid.Parse(data.EnvironmentId) : Guid.Empty
                    };
                    objects.Add(object2D);
                }

                return objects;
            }
        }






        // Create a new object
        public async Task<Object2d> CreateAsync(Object2d object2d)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {

                await sqlConnection.OpenAsync();
                object2d.Id = Guid.NewGuid();
                var query = "INSERT INTO [Object2D] (Id, PrefabId, PositionX, PositionY, ScaleX, ScaleY, RotationZ, SortingLayer, EnvironmentId) " +
                            "VALUES (@Id, @PrefabId, @PositionX, @PositionY, @ScaleX, @ScaleY, @RotationZ, @SortingLayer, @EnvironmentId); "
                          ;
                await sqlConnection.ExecuteAsync(query, object2d);

                // Assign the newly generated ID to the object
                return object2d;
            }
        }

        // Update an existing object
        public async Task UpdateAsync(Object2d object2d)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                var query = "UPDATE [Object2D] SET PrefabId = @PrefabId, PositionX = @PositionX, PositionY = @PositionY, " +
                            "ScaleX = @ScaleX, ScaleY = @ScaleY, RotationZ = @RotationZ, SortingLayer = @SortingLayer, " +
                            "EnvironmentId = @EnvironmentId WHERE Id = @Id";
                await sqlConnection.ExecuteAsync(query, object2d);
            }
        }

        // Delete an object by ID
        public async Task DeleteAsync(string id)
        {
            using (var sqlConnection = new SqlConnection(_sqlConnectionString))
            {
                await sqlConnection.OpenAsync();
                var query = "DELETE FROM [Object2D] WHERE Id = @Id";
                await sqlConnection.ExecuteAsync(query, new { Id = id });
            }
        }
    }
}
