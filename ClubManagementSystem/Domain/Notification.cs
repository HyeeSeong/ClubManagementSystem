namespace ClubManagementSystem.Domain;

public class Notification
{
    public int AnnounceID { get; set; }
    public string Description { get; set; } = "";
    public DateTime Date { get; set; } = DateTime.Today;

    public ICollection<Notifies> Notifies { get; set; } = new List<Notifies>();
}