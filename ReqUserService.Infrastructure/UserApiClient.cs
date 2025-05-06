using Microsoft.Extensions.Options;
using ReqUserService.Infrastructure.Config;
using Newtonsoft.Json;
using System.Net.Http;
using ReqUserService.Domain.Models;
using ReqUserService.Domain.Exceptions;
using System.Net;
using ReqUserService.Domain.Interfaces;

namespace ReqUserService.Infrastructure
{
    public class UserApiClient: IUserApiClient
    {
        private readonly HttpClient _httpClient;
        private readonly ReqUserApiOptions _apiOptions;
       
        public UserApiClient(HttpClient httpClient, IOptions<ReqUserApiOptions> apiOptions)
        {
            _httpClient = httpClient;
            _apiOptions = apiOptions.Value;
           
        }

        /// <summary>
        /// Get the user by Id
        /// </summary>
        /// <param name="userId">Id of the User</param>
        public async Task<User> GetUserByIdAsync(int userId)
        {
            try
            {
                
                var response = await _httpClient.GetAsync($"{_apiOptions.BaseUrl}users/{userId}");

                if (response.StatusCode == HttpStatusCode.NotFound)
                    throw new UserNotFoundException(userId);

                if (!response.IsSuccessStatusCode)
                    throw new ApiException($"Failed to fetch user: {response.ReasonPhrase}", response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                var wrapper = JsonConvert.DeserializeObject<JsonData>(content);

                return wrapper.Data;
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException($"HTTP request failed: {ex.Message}", HttpStatusCode.ServiceUnavailable);
            }
            catch (TaskCanceledException ex)
            {
                throw new ApiException("Request timed out.", HttpStatusCode.RequestTimeout);
            }
            
        }

        /// <summary>
        /// Get the users by page numbers
        /// </summary>
        /// <param name="pageNumber">PageNumber</param>
        public async Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{_apiOptions.BaseUrl}users?page={pageNumber}");

                if (!response.IsSuccessStatusCode)
                    throw new ApiException($"Failed to fetch users: {response.ReasonPhrase}", response.StatusCode);

                var content = await response.Content.ReadAsStringAsync();
                var page = JsonConvert.DeserializeObject<PageResponse>(content);
                return page.Data;
            }
            catch (HttpRequestException ex)
            {
                throw new ApiException($"HTTP request failed: {ex.Message}", HttpStatusCode.ServiceUnavailable);
            }
            catch (TaskCanceledException ex)
            {
                throw new ApiException("Request timed out.", HttpStatusCode.RequestTimeout);
            }
        }
    }

    public class PageResponse
    {
        public List<User> Data { get; set; }
        public int TotalPages { get; set; }
    }
}
