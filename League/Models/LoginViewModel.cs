using League.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace League.Models
{
    public class LoginViewModel
    {
        [Required]
        [EmailAddress]  
        public string Username { get; set; }


        [Required]
        [MinLength(6)]
        public string Password { get; set; }


        public bool RememberMe { get; set; }


    }
}
