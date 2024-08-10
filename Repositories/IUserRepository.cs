using AuthAPI.Models;

namespace AuthAPI.Repositories
{
    public interface IUserRepository
    {
        Task<List<User>> GetUsers();
        Task<User> CreateUser(UserRegisterModel model);
        Task<string> Login(UserLoginModel model);
    }
}
