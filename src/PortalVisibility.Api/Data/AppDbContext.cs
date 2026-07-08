using Microsoft.EntityFrameworkCore;
using PortalVisibility.Api.Models;

namespace PortalVisibility.Api.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Shareholder> Shareholders => Set<Shareholder>();
    public DbSet<Group> Groups => Set<Group>();
    public DbSet<Membership> Memberships => Set<Membership>();
    public DbSet<ContentItem> ContentItems => Set<ContentItem>();
    public DbSet<ContentAudienceGroup> ContentAudienceGroups => Set<ContentAudienceGroup>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // Composite keys for the two join tables.
        modelBuilder.Entity<Membership>()
            .HasKey(m => new { m.ShareholderId, m.GroupId });

        modelBuilder.Entity<Membership>()
            .HasOne(m => m.Shareholder)
            .WithMany(s => s.Memberships)
            .HasForeignKey(m => m.ShareholderId);

        modelBuilder.Entity<Membership>()
            .HasOne(m => m.Group)
            .WithMany(g => g.Memberships)
            .HasForeignKey(m => m.GroupId);

        modelBuilder.Entity<ContentAudienceGroup>()
            .HasKey(cag => new { cag.ContentItemId, cag.GroupId });

        modelBuilder.Entity<ContentAudienceGroup>()
            .HasOne(cag => cag.ContentItem)
            .WithMany(c => c.ContentAudienceGroups)
            .HasForeignKey(cag => cag.ContentItemId);

        modelBuilder.Entity<ContentAudienceGroup>()
            .HasOne(cag => cag.Group)
            .WithMany(g => g.ContentAudienceGroups)
            .HasForeignKey(cag => cag.GroupId);

        base.OnModelCreating(modelBuilder);
    }
}
