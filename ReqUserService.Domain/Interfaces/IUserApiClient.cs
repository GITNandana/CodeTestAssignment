using ReqUserService.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReqUserService.Domain.Interfaces
{
    public interface IUserApiClient
    {
        Task<User> GetUserByIdAsync(int userId);
        Task<IEnumerable<User>> GetAllUsersAsync(int pageNumber);



    }
}
