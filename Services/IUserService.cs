using AuthAPI.Models;

namespace AuthAPI.Services
{
    public interface IUserService
    {
        Task<User> CreateUser(UserRegisterModel model);
        Task<List<User>> GetUsers();
        Task<string> Login(UserLoginModel model);
    }
}