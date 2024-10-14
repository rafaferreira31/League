using System.ComponentModel.DataAnnotations;

namespace League.Models
{
    public class RegisterNewUserViewModel
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


        [MaxLength(20, ErrorMessage = "The field {0} only can contain {1} characteres lenght.")]

        public string PhoneNumber { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }


        [Required]
        [Compare("Password")]
        public string Confirm { get; set; }


        [Required]
        [Display(Name = "Role")]
        public string SelectedRole { get; set; }


        [Display(Name = "Profile Image")]
        public IFormFile? ImageFile { get; set; }
    }
}
