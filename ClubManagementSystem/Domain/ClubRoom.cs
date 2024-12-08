namespace ClubManagementSystem.Domain;

public class ClubRoom
{
    public int ClubRoomID { get; set; }
    public string Location { get; set; } = null!;
    public int Size { get; set; }
    public string Status { get; set; } = null!;
    
    public ICollection<Club>? Clubs { get; set; }
}