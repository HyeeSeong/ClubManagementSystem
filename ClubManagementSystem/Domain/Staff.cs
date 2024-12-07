namespace ClubManagementSystem.Domain;

public class Staff : User
{
    // Staff 전용 속성
    public string Position { get; set; } = "";
}