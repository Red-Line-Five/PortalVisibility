using Microsoft.EntityFrameworkCore;
using PortalVisibility.Api.Data;
using PortalVisibility.Api.Dtos;
using PortalVisibility.Api.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("PortalVisibilityDb"));

builder.Services.AddScoped<IContentVisibilityService, ContentVisibilityService>();
builder.Services.AddEndpointsApiExplorer();

var app = builder.Build();

// Seed the in-memory DB once at startup.
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    SeedData.Seed(db);
}

// --- Fake auth ---------------------------------------------------------
// No real auth for this exercise. The caller identifies themselves via
// an "X-Shareholder-Id" header. If it's missing or doesn't match a known
// shareholder, we return 401. This is intentionally simple; a real
// implementation would replace this with JWT/session-based auth and pull
// the shareholder id from the validated identity instead of a raw header.
static async Task<int?> GetShareholderIdAsync(HttpRequest request, AppDbContext db)
{
    if (!request.Headers.TryGetValue("X-Shareholder-Id", out var raw))
        return null;

    if (!int.TryParse(raw, out var id))
        return null;

    var exists = await db.Shareholders.AnyAsync(s => s.Id == id);
    return exists ? id : null;
}

// GET /api/content?limit=&offset=&q=
// Returns the caller's own visible content list, paginated.
app.MapGet("/api/content", async (
    HttpRequest request,
    AppDbContext db,
    IContentVisibilityService visibility,
    int limit = 20,
    int offset = 0,
    string? q = null) =>
{
    var shareholderId = await GetShareholderIdAsync(request, db);
    if (shareholderId is null)
        return Results.Unauthorized();

    limit = Math.Clamp(limit, 1, 100);
    offset = Math.Max(offset, 0);

    var groupIds = await db.Memberships
        .Where(m => m.ShareholderId == shareholderId)
        .Select(m => m.GroupId)
        .ToListAsync();

    var items = await db.ContentItems
        .Include(c => c.ContentAudienceGroups)
        .OrderByDescending(c => c.CreatedAt)
        .ToListAsync();

    var visibleItems = items
        .Where(item => visibility.IsVisible(item, groupIds))
        .Where(item => string.IsNullOrWhiteSpace(q)
            || item.Title.Contains(q, StringComparison.OrdinalIgnoreCase))
        .ToList();

    var page = visibleItems
        .Skip(offset)
        .Take(limit)
        .Select(item => new ContentListItemDto(item.Id, item.Title, item.IsPublic, item.CreatedAt))
        .ToList();

    var result = new PagedResult<ContentListItemDto>(page, visibleItems.Count, limit, offset);
    var json = System.Text.Json.JsonSerializer.Serialize(result);
    return Results.Content(json, "application/json");
});

// GET /api/content/{id}
// Returns the item if visible to the caller, 403 if it exists but is
// restricted away from them, 404 if it doesn't exist at all.
app.MapGet("/api/content/{id:int}", async (
    int id,
    HttpRequest request,
    AppDbContext db,
    IContentVisibilityService visibility) =>
{
    var shareholderId = await GetShareholderIdAsync(request, db);
    if (shareholderId is null)
        return Results.Unauthorized();

    var item = await db.ContentItems
        .Include(c => c.ContentAudienceGroups)
        .FirstOrDefaultAsync(c => c.Id == id);

    if (item is null)
        return Results.NotFound();

    var groupIds = await db.Memberships
        .Where(m => m.ShareholderId == shareholderId)
        .Select(m => m.GroupId)
        .ToListAsync();

    if (!visibility.IsVisible(item, groupIds))
        return Results.StatusCode(StatusCodes.Status403Forbidden);

    var dto = new ContentDetailDto(item.Id, item.Title, item.Body, item.IsPublic, item.CreatedAt);
    return Results.Ok(dto);
});

app.Run();

// Exposed for WebApplicationFactory in integration-style tests, if added later.
public partial class Program { }
