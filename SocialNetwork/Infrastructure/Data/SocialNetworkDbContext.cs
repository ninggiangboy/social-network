using Domain.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SharedKernel.SeedWorks;

namespace Infrastructure.Data;

public class SocialNetworkDbContext(DbContextOptions<SocialNetworkDbContext> options)
    : DbContext(options)
{
    public virtual DbSet<Profile> Profiles { get; init; }
    public virtual DbSet<Post> Posts { get; init; }
    public virtual DbSet<Reply> Replies { get; init; }
    public virtual DbSet<Like> Likes { get; init; }
    public virtual DbSet<Follow> Follows { get; init; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // foreach (var entityType in modelBuilder.Model.GetEntityTypes())
        // {
        //     if (typeof(ISoftDelete).IsAssignableFrom(entityType.ClrType))
        //     {
        //         modelBuilder.Entity(entityType.ClrType)
        //             .HasQueryFilter((ISoftDelete e) => !e.IsDeleted);
        //     }
        // }

        modelBuilder.Entity<Follow>()
            .HasKey(f => new { f.FollowerId, f.FolloweeId });
        // modelBuilder.Entity<Like>()
        //     .HasIndex(l => new { l.ProfileId, l.PostId })
        //     .IsUnique()
        //     .HasFilter("Likes.PostId IS NOT NULL");
        // modelBuilder.Entity<Like>()
        //     .HasIndex(l => new { l.ProfileId, l.ReplyId })
        //     .IsUnique()
        //     .HasFilter("Likes.ReplyId IS NOT NULL");
        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Follower)
            .WithMany(p => p.Following)
            .HasForeignKey(f => f.FollowerId)
            .OnDelete(DeleteBehavior.Restrict);
        modelBuilder.Entity<Follow>()
            .HasOne(f => f.Followee)
            .WithMany(p => p.Followers)
            .HasForeignKey(f => f.FolloweeId)
            .OnDelete(DeleteBehavior.Restrict);
    }

    public override int SaveChanges()
    {
        foreach (var entry in ChangeTracker.Entries<IAuditing>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                    break;
            }
        }

        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        foreach (var entry in ChangeTracker.Entries<IAuditing>())
        {
            switch (entry.State)
            {
                case EntityState.Added:
                    entry.Entity.CreatedAt = DateTimeOffset.UtcNow;
                    break;
                case EntityState.Modified:
                    entry.Entity.UpdatedAt = DateTimeOffset.UtcNow;
                    break;
            }
        }

        return base.SaveChangesAsync(cancellationToken);
    }
}