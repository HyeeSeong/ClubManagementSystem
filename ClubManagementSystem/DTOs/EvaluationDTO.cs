namespace ClubManagementSystem.DTOs;

public class EvaluationDTO
{
    public int EvaluationID { get; set; }
    public int ProfessorID { get; set; }
    public string? ProfessorName { get; set; }
    public int ProjectID { get; set; }
    public string? ProjectName { get; set; }
    public decimal Score { get; set; }
    public DateTime Date { get; set; }
}