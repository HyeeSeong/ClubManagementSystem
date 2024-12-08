namespace ClubManagementSystem.Domain;

public class Professor
{
    public int UserID { get; set; }

    public User User { get; set; } = null!;
    public ICollection<Club>? Clubs { get; set; }
    public ICollection<Evaluation>? Evaluations { get; set; }
}