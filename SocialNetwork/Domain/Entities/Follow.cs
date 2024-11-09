using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedKernel.SeedWorks;

namespace Domain.Entities;

public class Follow : IAuditing
{
    [Key, Column(Order = 0)] public string FollowerId { get; set; }
    [Key, Column(Order = 1)] public string FolloweeId { get; set; }
    public virtual Profile Follower { get; set; } = null!;
    public virtual Profile Followee { get; set; } = null!;
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}