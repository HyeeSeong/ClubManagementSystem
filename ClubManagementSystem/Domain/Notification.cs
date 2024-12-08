namespace ClubManagementSystem.Domain;

public class Notification
{
    public int AnnounceID { get; set; }
    public string Description { get; set; } = null!;
    public DateTime Date { get; set; }

    public ICollection<Notifies>? Notifies { get; set; }
}