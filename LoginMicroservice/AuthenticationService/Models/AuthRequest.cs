namespace AuthenticationService.Models;

public class AuthRequest
{
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public string Name { get; set; } = string.Empty;
    public string Address { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string Language { get; set; } = string.Empty;
}