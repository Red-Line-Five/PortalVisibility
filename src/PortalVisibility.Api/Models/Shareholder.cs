namespace PortalVisibility.Api.Models;

public class Shareholder
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;

    public List<Membership> Memberships { get; set; } = new();
}
