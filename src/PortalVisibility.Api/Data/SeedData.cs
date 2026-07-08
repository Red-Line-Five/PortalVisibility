using PortalVisibility.Api.Models;

namespace PortalVisibility.Api.Data;


public static class SeedData
{
    public static void Seed(AppDbContext db)
    {
        if (db.Shareholders.Any()) return;

        var p1 = new Shareholder { Id = 1, Name = "person 1", Email = "p1@example.com" };
        var p2 = new Shareholder { Id = 2, Name = "person 2", Email = "p2example.com" };
        var p3 = new Shareholder { Id = 3, Name = "person 3", Email = "p3@example.com" };

        var seriesA = new Group { Id = 1, Name = "Series A" };
        var boardObservers = new Group { Id = 2, Name = "Board Observers" };

        db.Shareholders.AddRange(p1, p2, p3);
        db.Groups.AddRange(seriesA, boardObservers);

        db.Memberships.AddRange(
            new Membership { ShareholderId = p1.Id, GroupId = seriesA.Id },
            new Membership { ShareholderId = p3.Id, GroupId = boardObservers.Id }
            // Carol belongs to no groups - only public content should be visible to her.
        );

        var publicAnnouncement = new ContentItem
        {
            Id = 1,
            Title = "Q2 Company Update",
            Body = "General update visible to all shareholders.",
            IsPublic = true
        };

        var seriesAOnly = new ContentItem
        {
            Id = 2,
            Title = "Series A Term Sheet Draft",
            Body = "Restricted to Series A investors.",
            IsPublic = false
        };

        var boardOnly = new ContentItem
        {
            Id = 3,
            Title = "Board Meeting Minutes",
            Body = "Restricted to Board Observers.",
            IsPublic = false
        };

        db.ContentItems.AddRange(publicAnnouncement, seriesAOnly, boardOnly);

        db.ContentAudienceGroups.AddRange(
            new ContentAudienceGroup { ContentItemId = seriesAOnly.Id, GroupId = seriesA.Id },
            new ContentAudienceGroup { ContentItemId = boardOnly.Id, GroupId = boardObservers.Id }
        );

        db.SaveChanges();
    }
}
