namespace ClubManagementSystem.DTOs;

public class ParticipationDTO
{
    public int StudentID { get; set; }
    public string? StudentName { get; set; } // Student의 FirstName + LastName 등
    public int ProjectID { get; set; }
    public string? ProjectName { get; set; }
}