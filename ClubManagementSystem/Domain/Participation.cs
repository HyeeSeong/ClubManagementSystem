namespace ClubManagementSystem.Domain;

public class Participation
{
    public int StudentID { get; set; }
    public int ProjectID { get; set; }

    public Student Student { get; set; } = null!;
    public Project Project { get; set; } = null!;
}