namespace ClubManagementSystem.Domain;

public class Participation
{
    public int StudentID { get; set; }
    public int ProjectID { get; set; }

    // Navigation Properties
    public Student? Student { get; set; }
    public Project? Project { get; set; }
}