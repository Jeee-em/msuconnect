using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Account
{
    public class NewUserDto
    {
        public string? Username { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }
        public string? Token { get; set; }
        public string? Role { get; set; }
        public string? Bio { get; set; }
        public string? ProfilePicture { get; set; }
        public bool IsAvailableForSession { get; set; } = true;
        public DateTime DateOfBirth { get; set; }
        public List<int>? ExpertiseIds { get; set; }
    }
}