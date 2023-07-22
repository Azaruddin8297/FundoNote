using FundoNotApplication.Entities;
using FundoNotApplication.Interface;
using FundoNotApplication.Models;
using FundoNotApplication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FundoNotApplication.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUser _userServices;

        public UserController(IUser userServices)
        {
            _userServices = userServices;
        }

        [HttpPost]
        [Route("Register")]

        public IActionResult Register(UserRegistration newUser)
        {
            UserEntity user = _userServices.Register(newUser);
            if (user != null)
            {
                return Ok(new {success = true,  message = "User Registration Successfull", data = user});
            }
            else
            {
                return BadRequest(new { success = false, message = "User Registration UnSuccessfull"});
            }
        }
        [HttpPost]
        [Route("Login")]
        public IActionResult Login(string email, string password)
        {
            string result = _userServices.LogIn(email, password);
            if (result != null)
            {
                return Ok(new { success = true, message = "Login Successfull", data = result });
            }
            else
            {
                return BadRequest(new { success = false, message = "Login UnSuccessfull" });
            }

        }

        [HttpPost]
        [Route("ForgetPassword")]
        public IActionResult ForgetPassword(string email)
        {
            bool result = _userServices.ForgetPassword(email);

            if(result)
                return Ok(new { success = true, message = "Reset Email Sent " });
            else
                return BadRequest(new { success = false, message = "Something went wrong" });
        }
        [Authorize]
        [HttpPut]
        [Route("ResetPassword")]
        public IActionResult ResetPassword(string newPassword, string comfirmPassword)
        {
            string emailId = User.FindFirstValue(ClaimTypes.Email);
            bool result = _userServices.ResetPassword(newPassword, emailId, comfirmPassword);

            if (result)
                return Ok(new { success = true, message = "Password Updated" });
            else
                return BadRequest(new { success = false, message = "Something went wrong" });
        }
    }
}
