namespace PortalVisibility.Api.Dtos;

public record ContentListItemDto(int Id, string Title, bool IsPublic, DateTime CreatedAt);

public record ContentDetailDto(int Id, string Title, string Body, bool IsPublic, DateTime CreatedAt);

public record PagedResult<T>(IEnumerable<T> Items, int Total, int Limit, int Offset);
