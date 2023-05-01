using Order.Application.DataContract.Request.User;
using Order.Domain.Models;
using Order.Domain.Validations.Base;
using System.Threading.Tasks;

namespace Order.Application.Interfaces
{
    public interface IUserApplication
    {
        Task<Response<AuthResponse>> AuthAsync(AuthRequest auth);
        Task<Response> CreateAsync(CreateUserRequest User)
    }
}
