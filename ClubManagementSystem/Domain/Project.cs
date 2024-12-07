namespace ClubManagementSystem.Domain;

public class Project
{
    public int ProjectID { get; set; }
    public string Name { get; set; } = "";
    public int ClubID { get; set; }

    // Navigation
    public Club? Club { get; set; }
    public ICollection<Participation> Participations { get; set; } = new List<Participation>();
    public ICollection<Evaluation> Evaluations { get; set; } = new List<Evaluation>();
    public ICollection<Report> Reports { get; set; } = new List<Report>();
}