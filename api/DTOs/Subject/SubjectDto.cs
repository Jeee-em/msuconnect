using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using api.Models;

namespace api.DTOs.Subject
{
    public class SubjectDto
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string MentorId { get; set; }
        public User? Mentor { get; set; }
    }
}