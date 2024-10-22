using League.Data.Entities;
using System.ComponentModel.DataAnnotations;

namespace League.Models
{
    public class ChangeUserAdminViewModel : User
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }


        [Display(Name = "Profile Image")]
        public IFormFile? ImageFile { get; set; }

        [Required]
        [Display(Name = "Role")]
        public string SelectedRole { get; set; }


        public string FullName => $"{FirstName} {LastName}";  
    }
}
