using Microsoft.Extensions.Caching.Memory;
using ReqUserService.Domain.Interfaces;
using ReqUserService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqUserService.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUserApiClient _userApiClient;
        private readonly IMemoryCache _cache;
        private readonly TimeSpan _defaultCacheDuration = TimeSpan.FromMinutes(5);

        public UserService(IUserApiClient userApiClient, IMemoryCache memoryCache)
        {
            _userApiClient = userApiClient;
            _cache = memoryCache;
        }
        
        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                var cacheKey = $"User_{userId}";

                if (_cache.TryGetValue(cacheKey, out User cachedUser))
                {
                    return cachedUser;
                }
                var user = await _userApiClient.GetUserByIdAsync(userId);
                _cache.Set(cacheKey, user, _defaultCacheDuration);
                return user;
                 
            }
            catch
            {
                throw;
            }
        }

        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            try
            {
                const string cacheKey = "AllUsers";

                if (_cache.TryGetValue(cacheKey, out IEnumerable<User> cachedUsers))
                {
                    return cachedUsers;
                }

                var allUsers = new List<User>();
               
                for (int page = 1; page <= 2; page++)
                {
                    var pageUsers = await _userApiClient.GetAllUsersAsync(page);
                    allUsers.AddRange(pageUsers);
                }

                _cache.Set(cacheKey, allUsers, _defaultCacheDuration);
                return allUsers;
                
            }
            catch
            {
                throw;
            }
        }
    }
}
