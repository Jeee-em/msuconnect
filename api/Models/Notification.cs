using System;

namespace api.Models
{
    public class Notification
    {
        public int NotificationId { get; set; }
        public string? UserId { get; set; }
        public string Message { get; set; } = string.Empty;  // Default to empty string
        public DateTime DateTimeSent { get; set; } = DateTime.UtcNow; // Default to current UTC time
        public bool IsRead { get; set; } = false;  // Default to not read

        public User? User { get; set; }
    }

}
