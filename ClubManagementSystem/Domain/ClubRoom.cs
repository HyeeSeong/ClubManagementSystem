namespace ClubManagementSystem.Domain;

public class ClubRoom
{
    public int ClubRoomID { get; set; }
    public string Location { get; set; } = "";
    public int Size { get; set; }
    public string Status { get; set; } = "";

    // 필요하다면 Club과의 관계 추가 가능 (1:N)
    public ICollection<Club> Clubs { get; set; } = new List<Club>();
}