using Microsoft.AspNetCore.Identity;

namespace League.Data.Entities
{
    public class User : IdentityUser
    {
        public string FirstName { get; set; }


        public string LastName { get; set; }
    }
}
