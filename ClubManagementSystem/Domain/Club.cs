namespace ClubManagementSystem.Domain;

public class Club
{
    public int ClubID { get; set; }
    public string ClubName { get; set; } = null!;
    public int? ClubRoomID { get; set; }
    public int ProfessorID { get; set; }
    public int StaffID { get; set; }

    public ClubRoom? ClubRoom { get; set; }
    public Professor Professor { get; set; } = null!;
    public Staff Staff { get; set; } = null!;
    public ICollection<Student>? Students { get; set; }
    public ICollection<Project>? Projects { get; set; }

    public override string ToString()
    {
        return $"[동아리 번호 : {ClubID}, 동아리 이름 : {ClubName}]";
    }
}