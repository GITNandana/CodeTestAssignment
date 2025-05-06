using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using ReqUserService.Domain.Interfaces;
using ReqUserService.Infrastructure;
using ReqUserService.Infrastructure.Config;
using ReqUserService.Infrastructure.Services;
using System;
using System.Threading.Tasks;

namespace ReqUserService.ConsoleApp
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var host = CreateHostBuilder(args).Build();
            var userService = host.Services.GetRequiredService<IUserService>();

            // Test getting user by ID
            var user = await userService.GetUserByIdAsync(1);
            Console.WriteLine(" UserService GetUserByID Data : ");
            Console.WriteLine($"{user.First_Name} {user.Last_Name} - {user.Email}");

            // Test getting all users
            var users = await userService.GetAllUsersAsync();
            Console.WriteLine(" UserService GetAllUsers Data : ");
            foreach (var u in users)
            {
                Console.WriteLine($"{u.First_Name} {u.Last_Name}");
            }
            Console.Read();
            Console.Read();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureServices((hostContext, services) =>
                {
                    services.Configure<ReqUserApiOptions>(hostContext.Configuration.GetSection("ReqResApi"));
                    services.AddHttpClient<IUserApiClient,UserApiClient>((sp, client) =>
                    {
                        var options = sp.GetRequiredService<IOptions<ReqUserApiOptions>>().Value;
                        client.BaseAddress = new Uri(options.BaseUrl);
                        client.DefaultRequestHeaders.Add("x-api-key", "reqres-free-v1");
                        client.DefaultRequestHeaders.Add("Accept", "application/json");
                    }).AddPolicyHandler(PollyPolicies.GetRetryPolicy())
                     .AddPolicyHandler(PollyPolicies.GetCircuitBreakerPolicy())
                      .AddPolicyHandler(PollyPolicies.GetTimeoutPolicy()); 
                    services.AddScoped<IUserService, UserService>();
                    services.AddMemoryCache();
                });
    }
}
