
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Options;
using Moq;
using ReqUserService.Domain.Interfaces;
using ReqUserService.Domain.Models;
using ReqUserService.Infrastructure;
using ReqUserService.Infrastructure.Config;
using ReqUserService.Infrastructure.Services;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
namespace ReqUserService.Test
{
        public class UserServiceTests
        {
        private readonly Mock<IUserApiClient> _mockApiClient;
        private readonly IMemoryCache _memoryCache;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _mockApiClient = new Mock<IUserApiClient>();

            _memoryCache = new MemoryCache(new MemoryCacheOptions());
            _userService = new UserService(_mockApiClient.Object, _memoryCache);
        }
            [Fact]
            public async Task GetUserByIdAsync_ShouldReturnUser()
            {
            // Arrange
            var userId = 1;
            var expectedUser = new User { Id = userId, First_Name = "Nandana", Last_Name = "Test", Email = "test@reqres.in" };

            _memoryCache.Set($"User_{userId}", expectedUser);

            // Act
            var user = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.Equal(expectedUser.Id, user.Id);
            _mockApiClient.Verify(x => x.GetUserByIdAsync(It.IsAny<int>()), Times.Never);
        }

            [Fact]
            public async Task GetAllUsersAsync_ShouldReturnListOfUsers()
            {
            var userId = 2;
            var expectedUser = new User { Id = userId, First_Name = "New", Last_Name = "User", Email = "newuser@reqres.in" };

            _mockApiClient
                .Setup(x => x.GetUserByIdAsync(userId))
                .ReturnsAsync(expectedUser);

            // Act
            var user = await _userService.GetUserByIdAsync(userId);

            // Assert
            Assert.NotNull(user);
            Assert.Equal(userId, user.Id);
            _mockApiClient.Verify(x => x.GetUserByIdAsync(userId), Times.Once);
        }
        }
    }

