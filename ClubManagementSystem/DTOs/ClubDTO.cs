namespace ClubManagementSystem.DTOs;

public class ClubDTO
{
    public int ClubID { get; set; }
    public string ClubName { get; set; } = "";
    public int? ClubRoomID { get; set; }
    public string? ClubRoomLocation { get; set; }

    public int ProfessorID { get; set; }
    public string? ProfessorName { get; set; }

    public int StaffID { get; set; }
    public string? StaffName { get; set; }

    public int MemberCount { get; set; }
    public int ProjectCount { get; set; }
}