using AuthAPI.Models;
using AuthAPI.Repositories;

namespace AuthAPI.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        public UserService(IUserRepository obj)
        {
            _userRepository = obj;
        }

        public async Task<List<User>> GetUsers()
        {           
            return await _userRepository.GetUsers(); // It calls the repository method to get all users.
        }

        public async Task<User> CreateUser(UserRegisterModel model)
        {
            return await _userRepository.CreateUser(model); // It calls the repository method to create user.
        }

        public async Task<string> Login(UserLoginModel model)
        {
            return await _userRepository.Login(model); // It calls the repository method to login user.
        }
    }
}
