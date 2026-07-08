namespace PortalVisibility.Api.Models;


public class Membership
{
    public int ShareholderId { get; set; }
    public Shareholder Shareholder { get; set; } = null!;

    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
}
