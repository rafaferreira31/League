using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace League.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }


        public string LastName { get; set; }


        [Display(Name = "Profile Image")]
        public Guid ImageId { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public string ImageFullPath => ImageId == Guid.Empty
            ? $"https://nosleague.azurewebsites.net/images/noimage.png"
            : $"https://nosleague.blob.core.windows.net/users/{ImageId}";


        public int? ClubId { get; set; }
    }
}