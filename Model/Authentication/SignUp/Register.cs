using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Identity;

namespace RegisterDemo.Model.Authentication.SignUp
{
    public class Register 
    {
        [Required(ErrorMessage = "Name is required")]
        public string UserName { get; set; }

        [EmailAddress]
        [Required(ErrorMessage = "Email is required")]
        public string Email { get; set; }

        [PasswordPropertyText]
        [Required(ErrorMessage = "password is required")]
        public string Password { get; set; }

    }
}
