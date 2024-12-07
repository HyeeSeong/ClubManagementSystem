namespace ClubManagementSystem.DTOs;

public class NotificationDTO
{
    public int AnnounceID { get; set; }
    public string Description { get; set; } = "";
    public DateTime Date { get; set; }
    public int NotifiesCount { get; set; }
}