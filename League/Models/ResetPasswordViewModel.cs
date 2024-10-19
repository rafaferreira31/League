using System.ComponentModel.DataAnnotations;

namespace League.Models
{
    public class ResetPasswordViewModel
    {
        [Required]
        public string Token { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [MinLength(6)]
        public string Password { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Compare("Password")]
        public string ConfirmPassword { get; set; }
    }
}
