namespace CulinaryCommand.Models
{
    public class NotificationModel
    {
        public bool TaskNotifications { get; set; } = true;
        public bool ScheduleNotifications { get; set; } = true;
        public bool SystemNotifications { get; set; } = false;
        public bool PushNotifications { get; set; } = true;
        public bool UrgentOnly { get; set; } = false;
        public string QuietHoursStart { get; set; } = "22:00";
        public string QuietHoursEnd { get; set; } = "08:00";
    }
}