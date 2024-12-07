namespace ClubManagementSystem.DTOs;

public class ProjectDTO
{
    public int ProjectID { get; set; }
    public string Name { get; set; } = "";
    public int ClubID { get; set; }
    public string? ClubName { get; set; }
    public int ParticipationCount { get; set; }
    public int EvaluationCount { get; set; }
    public int ReportCount { get; set; }
}