using Microsoft.AspNetCore.Identity;
using SharedKernel.SeedWorks;

namespace Domain.Entities;

public class Profile : Entity<string>, IAuditing, ISoftDelete
{
    public string? AvatarUrl { get; set; }
    public string? Bio { get; set; }
    public string Email { get; set; }
    public string UserName { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public virtual ICollection<Post> Posts { get; init; } = new List<Post>();
    public virtual ICollection<Reply> Replies { get; init; } = new List<Reply>();
    public virtual ICollection<Follow> Followers { get; init; } = new List<Follow>();
    public virtual ICollection<Follow> Following { get; init; } = new List<Follow>();
    public virtual ICollection<Like> Likes { get; init; } = new List<Like>();
}