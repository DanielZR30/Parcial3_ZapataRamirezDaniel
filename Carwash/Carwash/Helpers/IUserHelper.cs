
using Carwash.DAL.Entities;
using Carwash.Models;
using Microsoft.AspNetCore.Identity;

namespace Carwash.Helpers
{
    public interface IUserHelper
    {
        Task<User> GetUserAsync(string email);

        
        Task<IdentityResult> AddUserAsync(User user,string password);

        //valida el rol
        Task CheckRoleAsync(string roleName);

        //add 
        Task AddUserToRoleAsync(User user,string roleName);

       //valida si esta agregado
        Task<bool> IsUserInRoleAsync(User user, string roleName);

        Task<SignInResult> LoginAsync(LoginViewModel loginViewModel);
        Task LogoutAsync();
    }   
}
