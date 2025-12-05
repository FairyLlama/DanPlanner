namespace Danplanner.Shared.Models;

public class AddonOption
{
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public bool Selected { get; set; }
}