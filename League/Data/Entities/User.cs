using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace League.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }


        public string LastName { get; set; }


        [Display(Name = "Profile Image")]
        public string? ImageUrl { get; set; }
    }
}
