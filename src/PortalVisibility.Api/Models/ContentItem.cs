namespace PortalVisibility.Api.Models;


public class ContentItem
{
    public int Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Body { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public bool IsPublic { get; set; }

    public List<ContentAudienceGroup> ContentAudienceGroups { get; set; } = new();
}
