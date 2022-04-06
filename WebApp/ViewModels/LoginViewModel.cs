using System;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Not specified Email!")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Not specified password!")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}