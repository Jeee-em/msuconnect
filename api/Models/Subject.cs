namespace api.Models
{
    public class Subject
    {
        public int SubjectId { get; set; }
        public string SubjectName { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string? MentorId { get; set; }

        public User? Mentor { get; set; }
    }
}
