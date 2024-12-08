namespace ClubManagementSystem.Domain;

public class Project
{
    public int ProjectID { get; set; }
    public string Name { get; set; } = null!;
    public int ClubID { get; set; }

    public Club Club { get; set; } = null!;
    public ICollection<Participation>? Participations { get; set; }
    public ICollection<Evaluation>? Evaluations { get; set; }
    public ICollection<Report>? Reports { get; set; }
}