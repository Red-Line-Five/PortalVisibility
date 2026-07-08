using PortalVisibility.Api.Models;

namespace PortalVisibility.Api.Services;

public interface IContentVisibilityService
{

    bool IsVisible(ContentItem item, IEnumerable<int> shareholderGroupIds);
}
