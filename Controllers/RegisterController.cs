using Azure;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using RegisterDemo.Model.Authentication.SignUp;
using RegisterDemo.Model;
using Response = RegisterDemo.Model.Response;
using RegisterDemo.Service.Model;
using RegisterDemo.Service.Service;
using NETCore.MailKit.Core;
//using IEmailService = RegisterDemo.Service.Service.IEmailService;
using Azure.Identity;
using System;
using IEmailService = RegisterDemo.Service.Service.IEmailService;

namespace RegisterDemo.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegisterController : ControllerBase
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailService _emailservice;
        //private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _configuration;

        public RegisterController(UserManager<IdentityUser> userManager,/* RoleManager<IdentityRole> roleManager*/ IConfiguration configuration, IEmailService emailservice)
        {
            _userManager = userManager;
            _emailservice = emailservice;
            //_roleManager = roleManager;
            _configuration = configuration;
        }

      

    [HttpPost("CreateRegister")]

    public async Task<IActionResult> Register([FromBody] Register registerUser /*string role*/)
    {
        //check user exist
        var UserExit = await _userManager.FindByEmailAsync(registerUser.Email);

        if (UserExit != null)
        {
            return StatusCode(StatusCodes.Status403Forbidden,
                new Response { Status = "Error", Message = "User already exists" });

        }

        IdentityUser user = new()
        {
            Email = registerUser.Email,
            SecurityStamp = Guid.NewGuid().ToString(),
            UserName = registerUser.UserName,
            TwoFactorEnabled = true,
        };

        var result = await _userManager.CreateAsync(user, registerUser.Password);

            //add token
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var confirmationLink = Url.Action(nameof(ConfirmEmail), "Register", new { token, email = user.Email }, Request.Scheme);
            var message = new Message(new string[] { user.Email! }, "Confirmation email link", confirmationLink!);
            _emailservice.SendEmail(message);

            return result.Succeeded

        ? StatusCode(StatusCodes.Status200OK,
        new Response { Status = "Success", Message = "User Created Successfully" })
        : StatusCode(StatusCodes.Status500InternalServerError,
        new Response { Status = "Error", Message = "User Failed to  Create " });


    }

    [HttpGet("Get")]
        public IActionResult TestEmail()
        {
            var message = new Message(new string[]
            {
            "karishmaasri01@gmail.com"
            },
            "test", "hello");
            _emailservice.SendEmail(message);
            return StatusCode(StatusCodes.Status200OK,
            new Response
            {
                Status = "Sucesss",
                Message = "Email Sent SuccessFully"
            });
        }

        [HttpGet("ConfirmEmail")]
        public async Task<IActionResult> ConfirmEmail(string token, string email)
        {

            var user = await _userManager.FindByEmailAsync(email);

            if (user != null)
            {

                var result = await _userManager.ConfirmEmailAsync(user, token);
                if (result.Succeeded)
                {

                    return StatusCode(StatusCodes.Status200OK,

                    new Response { Status = "Success", Message = "Email Verified Successfully" });
                }
            }

            return StatusCode(StatusCodes.Status500InternalServerError,

            new Response { Status = "Error", Message = "This user doesnot exist" });
        }

    }


}