namespace ClubManagementSystem.Domain;

public class User
{
    public int UserID { get; set; }
    public string FirstName { get; set; } = null!;
    public string LastName { get; set; } = null!;
    public string Email { get; set; } = null!;
    public string PhoneNumber { get; set; } = "000-0000-0000";
    public DateTime? Birth { get; set; }
    public string? Description { get; set; }

    public Student? Student { get; set; }
    public Professor? Professor { get; set; }
    public Staff? Staff { get; set; }
    public ICollection<Notifies>? Notifies { get; set; }
    
    public override string ToString()
    {
        return $"[User = {UserID}, Name = {FirstName + " " + LastName}, Email = {Email}, PhoneNumber = {PhoneNumber}, Birth = {Birth}, Description = {Description}] ";
    }
}
