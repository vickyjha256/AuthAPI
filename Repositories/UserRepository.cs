using AuthAPI.Data;
using AuthAPI.Models;
using AuthAPI.Repositories;
using Azure;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthAPI.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _config;
        public UserRepository(AppDbContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }


        public async Task<List<User>> GetUsers()
        {
            return await _context.Users.ToListAsync();
        }

        public async Task<User> CreateUser(UserRegisterModel model)
        {
            var exist = await _context.Users.AnyAsync(x => x.Email == model.Email);
            if (exist)
            {
                throw new Exception("Email already exist.");
            }


            var encryptedPassword = BCrypt.Net.BCrypt.EnhancedHashPassword(model.Password);
            var user = new User();

            user.Name = model.Name;
            user.Email = model.Email;
            //user.Password = model.Password;
            user.Password = encryptedPassword;

            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();

            return user;
        }

        public async Task<string> Login(UserLoginModel model)
        {
            var userExist = await _context.Users.Where(x => x.Email == model.Email).FirstOrDefaultAsync();
            if (userExist == null)
            {
                throw new Exception("User doesn't exist.");
            }

            var verifyPassword = BCrypt.Net.BCrypt.EnhancedVerify(model.Password, userExist.Password);
            if (!verifyPassword)
            {
                throw new Exception("Invalid password !!");
            }
            var Token = GenerateToken(userExist.Name);
            return Token;
        }

        public string GenerateToken(string UserName)
        {
            try
            {
                var key = _config["Jwt:Key"];

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier,UserName)
            };
            var token = new JwtSecurityToken(_config["Jwt:Issuer"],
                _config["Jwt:Audience"],
                claims,
                expires: DateTime.Now.AddMinutes(5),
                signingCredentials: credentials);
            return new JwtSecurityTokenHandler().WriteToken(token);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


    }


}
