namespace ClubManagementSystem.DTOs;

public class StudentDTO : UserDTO
{
    public string Status { get; set; } = "";
    public int Year { get; set; } = 1;
    public int? ClubID { get; set; }
    public string? ClubName { get; set; } // Student가 속한 Club의 이름 제공
}