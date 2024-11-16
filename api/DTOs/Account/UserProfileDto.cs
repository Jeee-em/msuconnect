using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace api.DTOs.Account
{
    public class UserProfileDto
    {
        [Required]
        public string? Username { get; set; }
        [Required]
        public string? FirstName { get; set; }
        [Required]
        public string? LastName { get; set; }
        [Required]
        [EmailAddress]
        public string? Email { get; set; }
        [Required]
        public string? Password { get; set; }
        [Required]
        public string? Role { get; set; }
        [Required]
        public string? Bio { get; set; }
        [Required]
        public string? ProfilePicture { get; set; }
        [Required]
        public bool IsAvailableForSession { get; set; } = true;
        [Required]
        public DateTime DateOfBirth { get; set; }
        public List<int>? ExpertiseIds { get; set; }
    }
}