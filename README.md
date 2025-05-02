# CodeTestAssignment



**ReqUserService**
A .NET Core class library for fetching and managing user data from the public ReqRes API, designed to demonstrate clean architecture, resilience, and testability.

**Features**

#API client using HttpClient & IHttpClientFactory
#Clean architecture (Domain, Infrastructure layers)
#Async/await and proper error handling
#Pagination handling to fetch all users
#Custom exceptions (UserNotFoundException, ApiException, etc.)
#In-memory caching with configurable expiration (using IMemoryCache)
#Extensible retry policy via Polly (commented or configured)
#Strongly typed configuration (using Options pattern)
#Unit tests with mocked dependencies
#Optional demo via console app

**Project Structure**

# ReqUserService.sln
# ReqUserService.Domain         # Core models,#Interfaces (e.g., User,Interfaces and service contracts)
# ReqUserService.Infrastructure # API client, caching, configuration
# ReqUserService.ConsoleApp    # Sample app showing usage
#  ReqUserService.Tests          # Unit tests using xUnit + Moq
**Setup Instructions**
Clone the repo

git clone https://github.com/GITNandana/CodeTestAssignment.git
cd ReqUserService
Run the Console Demo


dotnet run --project ReqUserService.ConsoleDemo
Run Tests

dotnet test

**Configuration**
Located in appsettings.json 
{
  "ReqUserApi": {
    "BaseUrl": "https://reqres.in/api/"
  }
}
**Key Interfaces**

public interface IUserApiClient
{
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
}

public interface IExternalUserService
{
    Task<User> GetUserByIdAsync(int id);
    Task<IEnumerable<User>> GetAllUsersAsync();
}

**Design Decisions**
Clean Architecture: Ensures separation of concerns between layers.

IHttpClientFactory: Promotes proper reuse and avoids socket exhaustion.

IMemoryCache: Speeds up repeated requests and reduces load on external APIs.

Polly (Commented): Can be enabled to add retries for transient failures.

Custom Exceptions: Makes it easier to handle errors explicitly in consuming code.

Moq & xUnit: For isolated and testable services.

**Future Improvements**
Add retry policies with exponential backoff using Polly.

Add logging with Serilog or Microsoft.Extensions.Logging.

Add integration tests with live ReqRes API (rate-limited).

