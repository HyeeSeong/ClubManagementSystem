namespace ClubManagementSystem.Domain;

public class Report
{
    public int ReportID { get; set; }
    public int ProjectID { get; set; }
    public DateTime Date { get; set; }
    public string? Description { get; set; }

    public Project? Project { get; set; }
}