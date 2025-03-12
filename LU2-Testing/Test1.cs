using LU2.Models;
using Microsoft.Data.SqlClient;
using Moq;
using ProjectMap.WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Data;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LU2_Testing
{
    [TestClass]
    public sealed class Test1
    {
        private Mock<SqlConnection> _mockSqlConnection;
        private Mock<IDbCommand> _mockDbCommand;
        private Mock<IDataReader> _mockDataReader;
        private EnviromentRepository _enviromentRepository;

        [TestInitialize]
        public void Setup()
        {
            _mockSqlConnection = new Mock<SqlConnection>();
            _mockDbCommand = new Mock<IDbCommand>();
            _mockDataReader = new Mock<IDataReader>();
            _enviromentRepository = new EnviromentRepository("fake-connection-string");
        }

        [TestMethod]
        public async Task GetAllEnvironments_ShouldReturnEnvironments()
        {
            // Arrange
            var userId = Guid.NewGuid();
            var environments = new List<Environment2D>
            {
                new Environment2D { Id = Guid.NewGuid(), Name = "Env1", MaxHeight = 10, MaxLength = 20, userId = userId },
                new Environment2D { Id = Guid.NewGuid(), Name = "Env2", MaxHeight = 15, MaxLength = 25, userId = userId }
            };

            _mockDataReader.SetupSequence(m => m.Read())
                .Returns(true)
                .Returns(true)
                .Returns(false);

            _mockDataReader.Setup(m => m["Id"]).Returns(environments[0].Id.ToString());
            _mockDataReader.Setup(m => m["Name"]).Returns(environments[0].Name);
            _mockDataReader.Setup(m => m["MaxHeight"]).Returns(environments[0].MaxHeight);
            _mockDataReader.Setup(m => m["MaxLength"]).Returns(environments[0].MaxLength);
            _mockDataReader.Setup(m => m["UserId"]).Returns(environments[0].userId.ToString());

            _mockDbCommand.Setup(m => m.ExecuteReader()).Returns(_mockDataReader.Object);
            _mockSqlConnection.Setup(m => m.CreateCommand()).Returns(_mockDbCommand.Object);

            // Act
            var result = await _enviromentRepository.GetAllEnvironments(userId.ToString());

            // Assert
            Assert.AreEqual(2, result.Count);
            Assert.AreEqual("Env1", result[0].Name);
            Assert.AreEqual("Env2", result[1].Name);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnEnvironment()
        {
            // Arrange
            var environmentId = Guid.NewGuid();
            var environment = new Environment2D { Id = environmentId, Name = "Env1", MaxHeight = 10, MaxLength = 20, userId = Guid.NewGuid() };

            _mockDataReader.SetupSequence(m => m.Read())
                .Returns(true)
                .Returns(false);

            _mockDataReader.Setup(m => m["Id"]).Returns(environment.Id.ToString());
            _mockDataReader.Setup(m => m["Name"]).Returns(environment.Name);
            _mockDataReader.Setup(m => m["MaxHeight"]).Returns(environment.MaxHeight);
            _mockDataReader.Setup(m => m["MaxLength"]).Returns(environment.MaxLength);
            _mockDataReader.Setup(m => m["UserId"]).Returns(environment.userId.ToString());

            _mockDbCommand.Setup(m => m.ExecuteReader()).Returns(_mockDataReader.Object);
            _mockSqlConnection.Setup(m => m.CreateCommand()).Returns(_mockDbCommand.Object);

            // Act
            var result = await _enviromentRepository.GetByIdAsync(environmentId.ToString());

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Env1", result.Name);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldCreateEnvironment()
        {
            // Arrange
            var environment = new Environment2D { Name = "Env1", MaxHeight = 10, MaxLength = 20, userId = Guid.NewGuid() };

            _mockDbCommand.Setup(m => m.ExecuteNonQuery()).Returns(1);
            _mockSqlConnection.Setup(m => m.CreateCommand()).Returns(_mockDbCommand.Object);

            // Act
            var result = await _enviromentRepository.CreateAsync(environment);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Env1", result.Name);
        }
    }
}
