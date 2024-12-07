namespace ClubManagementSystem.DTOs;

public class ReportDTO
{
    public int ReportID { get; set; }
    public int ProjectID { get; set; }
    public string? ProjectName { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }
}