namespace ClubManagementSystem.Domain;

public class Student : User
{
    // Student 전용 속성
    public string Status { get; set; } = "";
    public int Year { get; set; } = 1;

    // 외래키
    public int? ClubID { get; set; }

    // Navigation Properties
    public Club? Club { get; set; }
    public ICollection<Participation> Participations { get; set; } = new List<Participation>();
}