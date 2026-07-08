namespace PortalVisibility.Api.Models;


public class ContentAudienceGroup
{
    public int ContentItemId { get; set; }
    public ContentItem ContentItem { get; set; } = null!;

    public int GroupId { get; set; }
    public Group Group { get; set; } = null!;
}
