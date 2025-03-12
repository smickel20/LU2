using LU2.Models;
using Moq;
using ProjectMap.WebApi.Repositories;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace LU2_Testing
{
    [TestClass]
    public sealed class Test1
    {
        private Mock<IEnviromentRepository> _mockEnviromentRepository;

        [TestInitialize]
        public void Setup()
        {
            _mockEnviromentRepository = new Mock<IEnviromentRepository>();
        }

        [TestMethod]
        public async Task GetAllEnvironments_ShouldReturnEnvironments()
        {
            // Arrange
            var userId = Guid.NewGuid().ToString();
            var environments = new List<Environment2D>
            {
                new Environment2D { Id = Guid.NewGuid(), Name = "Env1", MaxHeight = 10, MaxLength = 20, userId = Guid.Parse(userId) },
                new Environment2D { Id = Guid.NewGuid(), Name = "Env2", MaxHeight = 15, MaxLength = 25, userId = Guid.Parse(userId) }
            };

            _mockEnviromentRepository.Setup(repo => repo.GetAllEnvironments(userId))
                .ReturnsAsync(environments);

            // Act
            var result = await _mockEnviromentRepository.Object.GetAllEnvironments(userId);

            // Assert
            Assert.IsNotNull(result);
        }

        [TestMethod]
        public async Task GetByIdAsync_ShouldReturnEnvironment()
        {
            // Arrange
            var environmentId = Guid.NewGuid().ToString();
            var environment = new Environment2D { Id = Guid.Parse(environmentId), Name = "Env1", MaxHeight = 10, MaxLength = 20, userId = Guid.NewGuid() };

            _mockEnviromentRepository.Setup(repo => repo.GetByIdAsync(environmentId))
                .ReturnsAsync(environment);

            // Act
            var result = await _mockEnviromentRepository.Object.GetByIdAsync(environmentId);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Env1", result.Name);
        }

        [TestMethod]
        public async Task CreateAsync_ShouldCreateEnvironment()
        {
            // Arrange
            var environment = new Environment2D { Name = "Env1", MaxHeight = 10, MaxLength = 20, userId = Guid.NewGuid() };

            _mockEnviromentRepository.Setup(repo => repo.CreateAsync(environment))
                .ReturnsAsync(environment);

            // Act
            var result = await _mockEnviromentRepository.Object.CreateAsync(environment);

            // Assert
            Assert.IsNotNull(result);
            Assert.AreEqual("Env1", result.Name);
        }
    }

    public interface IEnviromentRepository
    {
        Task<IEnumerable<Environment2D>> GetAllEnvironments(string email);
        Task<Environment2D?> GetByIdAsync(string id);
        Task<Environment2D> CreateAsync(Environment2D environment);
        Task UpdateAsync(Environment2D environment);
        Task DeleteAsync(string id);
    }
}
