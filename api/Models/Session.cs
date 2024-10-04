using System;

namespace api.Models
{
    public class Session
    {
        public int SessionId { get; set; }
        public DateTime ScheduledAt { get; set; } = DateTime.UtcNow; // Default to current UTC time
        public int SubjectId { get; set; }
        public string? StudentId { get; set; }
        public string? MentorId { get; set; }
        public bool IsCompleted { get; set; } = false;
        public User? Mentor { get; set; }
        public User? Student { get; set; }
        public Subject? Subject { get; set; }
    }
}
