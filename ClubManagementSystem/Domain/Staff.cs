namespace ClubManagementSystem.Domain;

public class Staff
{
    public int UserID { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Club>? Clubs { get; set; }
}