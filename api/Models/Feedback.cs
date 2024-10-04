namespace api.Models
{
    public class Feedback
    {
        public int? FeedbackId { get; set; }
        public string? Comment { get; set; }
        public int SessionId { get; set; }

        public Session? Session { get; set; }
    }
}
