using PortalVisibility.Api.Models;

namespace PortalVisibility.Api.Services;

public class ContentVisibilityService : IContentVisibilityService
{
    public bool IsVisible(ContentItem item, IEnumerable<int> shareholderGroupIds)
    {
        if (item.IsPublic)
            return true;

        var allowedGroupIds = item.ContentAudienceGroups.Select(cag => cag.GroupId);
        return allowedGroupIds.Intersect(shareholderGroupIds).Any();
    }
}
