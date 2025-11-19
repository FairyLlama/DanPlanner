namespace AuthenticationService.Models;

public class AuthRequest
{
    public required string Email { get; set; }
    public required string Password { get; set; }

    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string Phone { get; set; }
    public required string Country { get; set; }
    public required string Language { get; set; }
}