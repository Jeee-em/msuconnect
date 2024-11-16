using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.DTOs.Account;
using api.Models;

namespace api.Mappers
{
    public static class AccountMapper
    {
        public static UserProfileDto ToUserProfileDto(this User user)
        {
            return new UserProfileDto
            {
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.PasswordHash,
                Role = user.Role,
                Bio = user.Bio,
                ProfilePicture = user.ProfilePicture,
                IsAvailableForSession = user.IsAvailableForSession,
                DateOfBirth = user.DateOfBirth,
                ExpertiseIds = user.Expertise.Select(s => s.SubjectId).ToList()
            };
        }

        public static UpdateProfileDto ToUpdateProfileDto(this User user)
        {
            return new UpdateProfileDto
            {
                Username = user.UserName,
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Password = user.PasswordHash,
                Role = user.Role,
                Bio = user.Bio,
                ProfilePicture = user.ProfilePicture,
                IsAvailableForSession = user.IsAvailableForSession,
                DateOfBirth = user.DateOfBirth,
                ExpertiseIds = user.Expertise.Select(s => s.SubjectId).ToList()
            };
        }
    }
}