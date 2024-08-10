using AuthAPI.Data;
using AuthAPI.Models;
using AuthAPI.Services;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace AuthAPI.Controllers
{
    [ApiController]
    //[Route("api/User")] // Here we gave the route name. Below is the another way of giving route name.
    [Route("api/[controller]")] // It will act same as above because it takes the controller name. Since controller name is same as above.
    public class UserAuthController : Controller
    {
        private readonly IUserService user_service;

        public UserAuthController(IUserService obj)
        {
            this.user_service = obj;
        }

        //[HttpGet]
        //public async Task<IActionResult> GetUsers()
        //{
        //    try
        //    {
        //        //return Ok(await _context.Users.ToListAsync());
        //        var users = await user_service.GetUsers();
        //        return Ok(users);
        //    }
        //    catch (Exception ex)
        //    {
        //        return BadRequest(new { message = ex.Message });
        //    }
        //}

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await user_service.GetUsers();
        }


        [HttpPost]
        public async Task<ActionResult> AddUser(UserRegisterModel model)
        {
            var user = await user_service.CreateUser(model);
            return Created();
        }

        [HttpPost("Login")]
        public async Task<ActionResult> LoginUser(UserLoginModel model)
        {
            var check = await user_service.Login(model);
            return Ok(check);
        }
    }
}
