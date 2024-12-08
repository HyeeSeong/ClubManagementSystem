namespace ClubManagementSystem.Domain;

public class Evaluation
{
    public int EvaluationID { get; set; }
    public int ProfessorID { get; set; }
    public int ProjectID { get; set; }
    public decimal Score { get; set; }
    public DateTime Date { get; set; }

    public Professor Professor { get; set; } = null!;
    public Project Project { get; set; } = null!;
}