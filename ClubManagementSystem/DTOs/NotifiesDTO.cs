namespace ClubManagementSystem.DTOs;

public class NotifiesDTO
{
    public int NotificationID { get; set; }
    public string? NotificationDescription { get; set; }
    public int UserID { get; set; }
    public string? UserName { get; set; } // User의 FirstName + LastName 등
}