using System.ComponentModel.DataAnnotations;
using Domain.Enums;
using SharedKernel.SeedWorks;

namespace Domain.Entities;

public class Post : Entity<long>, IAuditing, ISoftDelete
{
    public string ProfileId { get; set; }
    [StringLength(1000)] public string? Content { get; set; }
    public string[]? ImageUrls { get; set; }
    public PostVisibility Visibility { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public virtual Profile Profile { get; set; } = null!;
    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
}