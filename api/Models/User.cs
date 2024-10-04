using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace api.Models
{
    public class User : IdentityUser
    {

        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;
        public string Bio { get; set; } = string.Empty;

        public List<Subject> Expertise { get; set; } = new List<Subject>();
        public List<Session> Sessions { get; set; } = new List<Session>();
    }

}
