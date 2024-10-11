using System.ComponentModel.DataAnnotations;

namespace League.Models
{
    public class RegisterGuestViewModel
    {
        [Required]
        [Display(Name = "First Name")]
        public string FirstName { get; set; }


        [Required]
        [Display(Name = "Last Name")]
        public string LastName { get; set; }


        [Required]
        [DataType(DataType.EmailAddress)]
        public string Username { get; set; }


        [Required]
        public string PhoneNumber { get; set; }


        [Required]
        [MinLength(6)]
        public string Password { get; set; }


        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }
    }
}