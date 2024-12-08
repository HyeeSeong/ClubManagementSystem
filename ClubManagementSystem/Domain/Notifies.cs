namespace ClubManagementSystem.Domain;

public class Notifies
{
    public int NotificationID { get; set; }
    public int UserID { get; set; }

    public Notification Notification { get; set; } = null!;
    public User User { get; set; } = null!;
}