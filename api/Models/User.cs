using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class User : IdentityUser
    {
        public string? FirstName { get; set; } = string.Empty;
        public string? LastName { get; set; } = string.Empty;
        public string? FullName => $"{FirstName} {LastName}";
        public string? Role { get; set; } = string.Empty;
        public string? Bio { get; set; } = string.Empty;
        public string? ProfilePicture { get; set; }
        public bool IsAvailableForSession { get; set; } = true;
        public DateTime DateOfBirth { get; set; }

        public List<Subject?> Expertise { get; set; } = new List<Subject?>();
        public List<Session> Sessions { get; set; } = new List<Session>();
    }

}
