namespace ClubManagementSystem.Domain;

public class User
{
    public int UserID { get; set; } // PK
    public string FirstName { get; set; } = "";
    public string LastName { get; set; } = "";
    public string Email { get; set; } = "";
    public string PhoneNumber { get; set; } = "000-0000-0000";
    public DateTime? Birth { get; set; }
    public string? Description { get; set; }

    // Navigation Properties
    public ICollection<Notifies> Notifies { get; set; } = new List<Notifies>();
}
