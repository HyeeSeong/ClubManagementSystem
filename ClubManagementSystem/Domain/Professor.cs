namespace ClubManagementSystem.Domain;

public class Professor : User
{
    // Professor 전용 속성
    public string Department { get; set; } = "";
}