namespace ClubManagementSystem.DTOs;

public class ProfessorDTO : UserDTO
{
    // Professor는 Department 같은 속성이 없다고 했으나, 필요하다면 아래처럼 가능
    public string Department { get; set; } = "";
}