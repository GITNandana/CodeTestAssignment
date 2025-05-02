using ReqUserService.Domain;
using ReqUserService.Infrastructure;

services.Configure<ReqResApiOptions>(hostContext.Configuration.GetSection("ReqResApi"));
services.AddHttpClient<UserApiClient>();
services.AddSingleton<IUserService, ExternalUserService>();
