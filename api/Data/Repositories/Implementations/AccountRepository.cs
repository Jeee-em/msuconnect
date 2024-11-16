using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using api.Data.Repositories.Interfaces;
using api.DTOs.Account;
using api.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace api.Data.Repositories.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDBContext _context;
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        public AccountRepository(ApplicationDBContext context, UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _context = context;
            _signInManager = signInManager;
            _userManager = userManager;
        }
        public async Task AddUserToRoleAsync(User user, string role)
        {
            var roleClaim = new Claim(ClaimTypes.Role, role);
            await _userManager.AddClaimAsync(user, roleClaim);
        }

        public async Task<IdentityResult> CreateUserAsync(User user, string password)
        {
            return await _userManager.CreateAsync(user, password);
        }

        public async Task<User?> GetUserByIdAsync(string id)
        {
            return await _userManager.Users.Include(u => u.Expertise).FirstOrDefaultAsync(u => u.Id == id);
        }

        public async Task<User?> GetUserByUsernameAsync(string username)
        {
            return await _userManager.Users
            .Include(u => u.Expertise)
            .FirstOrDefaultAsync(u => u.UserName == username.ToLower());
        }

        public Task Hi()
        {
            throw new NotImplementedException();
        }

        public async Task<User?> UpdateUserProfileAsync(string id, UpdateProfileDto updateProfileDto)
        {
            var existingUserProfile = await _context.Users
                .Include(u => u.Expertise) // Include the current expertise list
                .FirstOrDefaultAsync(x => x.Id == id);

            if (existingUserProfile == null) return null;

            // Update basic user info
            existingUserProfile.UserName = updateProfileDto.Username;
            existingUserProfile.FirstName = updateProfileDto.FirstName;
            existingUserProfile.LastName = updateProfileDto.LastName;
            existingUserProfile.PasswordHash = updateProfileDto.Password;
            existingUserProfile.Email = updateProfileDto.Email;
            existingUserProfile.Bio = updateProfileDto.Bio;
            existingUserProfile.Role = updateProfileDto.Role;
            existingUserProfile.ProfilePicture = updateProfileDto.ProfilePicture;
            existingUserProfile.DateOfBirth = updateProfileDto.DateOfBirth;

            // Clear existing expertise and add the new ones
            existingUserProfile.Expertise.Clear();
            
            foreach (var subjectId in updateProfileDto.ExpertiseIds)
            {
                var subject = await _context.Subjects.FindAsync(subjectId);
                if (subject != null)
                {
                    existingUserProfile.Expertise.Add(subject);
                }
            }
            await _context.SaveChangesAsync();

            return existingUserProfile;
        }

    }
}