using SharedKernel.SeedWorks;

namespace Domain.Entities;

public class Reply : Entity<long>, IAuditing, ISoftDelete
{
    public long? PostId { get; set; }
    public long? ParentReplyId { get; set; }
    public string ProfileId { get; set; }
    public string Content { get; set; } = null!;

    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
    public bool IsDeleted { get; set; }

    public virtual Post? Post { get; set; }
    public virtual Profile? Profile { get; set; }
    public virtual Reply? ParentReply { get; set; }
    public virtual ICollection<Reply> Replies { get; set; } = new List<Reply>();
    public virtual ICollection<Like> Likes { get; set; } = new List<Like>();
}