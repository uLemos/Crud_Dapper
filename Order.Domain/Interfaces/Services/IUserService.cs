using Order.Domain.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Order.Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<bool> AuthenticationAsync(UserModel user);
        Task CreateAsync(UserModel user);
        Task UpdateAsync(UserModel user);
        Task DeleteAsync(string userId);
        Task<UserModel> GetByIdAsync(UserModel userId);
        Task<List<UserModel>> ListByFiltersAsync(string userId = null, string name = null);

    }
}
