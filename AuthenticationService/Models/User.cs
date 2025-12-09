namespace AuthenticationService.Models;

// klasse til at repræsentere en bruger
public class User
{
    public int Id { get; set; }
    public string Email { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
    public string Role { get; set; } = "Customer";
}