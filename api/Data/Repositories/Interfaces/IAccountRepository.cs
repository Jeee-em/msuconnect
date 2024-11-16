using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.Models;
using Microsoft.AspNetCore.Identity;

namespace api.Data.Repositories.Interfaces
{
    public interface IAccountRepository
    {
        Task<IdentityResult> CreateUserAsync(User user, string password);
        Task<User?> UpdateUserProfileAsync(string id, UpdateProfileDto updateProfileDto);
        Task AddUserToRoleAsync(User user, string roleName);
        Task<User?> GetUserByUsernameAsync(string username);
        Task<User?> GetUserByIdAsync(string id);
        Task Hi();
    }
}