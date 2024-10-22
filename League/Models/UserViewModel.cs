using League.Data.Entities;

namespace League.Models
{
    public class UserViewModel : User
    {
        public string FirstName { get; set; }

        public string LastName { get; set; }

        public string Username { get; set; }

        public string PhoneNumber { get; set; }

        public string Role { get; set; }  

        public string ImageFullPath { get; set; } 

        public string FullName => $"{FirstName} {LastName}";
    }
}
