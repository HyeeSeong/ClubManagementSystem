namespace ClubManagementSystem.Domain;

public class Club
{
    public int ClubID { get; set; } // PK
    public string ClubName { get; set; } = "";
    public int? ClubRoomID { get; set; }
    public int ProfessorID { get; set; }
    public int StaffID { get; set; }

    // Navigation Properties
    public ClubRoom? ClubRoom { get; set; }
    public Professor? Professor { get; set; }
    public Staff? Staff { get; set; }
    public ICollection<Student> Members { get; set; } = new List<Student>();
    public ICollection<Project> Projects { get; set; } = new List<Project>();
}