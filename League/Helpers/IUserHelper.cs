using League.Data.Entities;
using Microsoft.AspNetCore.Identity;

namespace League.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserByEmailAsync(string email);

        Task<IdentityResult> AddUserAsync(User user, string password);
    } 
}
