namespace PortalVisibility.Api.Models;


public class Group
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;

    public List<Membership> Memberships { get; set; } = new();
    public List<ContentAudienceGroup> ContentAudienceGroups { get; set; } = new();
}
