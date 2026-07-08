using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using PortalVisibility.Api.Dtos;

namespace PortalVisibility.Tests;

public class ContentVisibilityTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly WebApplicationFactory<Program> _factory;

    public ContentVisibilityTests(WebApplicationFactory<Program> factory)
    {
        _factory = factory;
    }

    [Fact]
    public async Task GetContentList_ReturnsVisibleItems_ForShareholder1()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Shareholder-Id", "1");

        var resp = await client.GetAsync("/api/content");

        Assert.Equal(HttpStatusCode.OK, resp.StatusCode);
        var paged = await resp.Content.ReadFromJsonAsync<PagedResult<ContentListItemDto>>();
        Assert.NotNull(paged);
        // Shareholder 1 belongs to Series A and should see at least the Series A item (id 2)
        Assert.Contains(paged!.Items, i => i.Id == 2);
    }

    [Fact]
    public async Task GetContentDetail_ReturnsForbidden_WhenNotInAllowedGroup()
    {
        var client = _factory.CreateClient();
        client.DefaultRequestHeaders.Add("X-Shareholder-Id", "3");

        // Content id 2 is Series A only; shareholder 3 is not in Series A
        var resp = await client.GetAsync("/api/content/2");

        Assert.Equal(HttpStatusCode.Forbidden, resp.StatusCode);
    }
}
