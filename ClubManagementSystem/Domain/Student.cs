namespace ClubManagementSystem.Domain;

public class Student
{
    public int UserID { get; set; }
    public string Status { get; set; } = null!;
    public int Year { get; set; } = 1;
    public int? ClubID { get; set; }

    public User User { get; set; } = null!;
    public Club? Club { get; set; }
    public ICollection<Participation>? Participations { get; set; }
}