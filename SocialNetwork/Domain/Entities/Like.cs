using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using SharedKernel.SeedWorks;

namespace Domain.Entities;

public class Like : Entity<long>
{
    public long? PostId { get; set; }
    public string ProfileId { get; set; }
    public long? ReplyId { get; set; }
    public Post? Post { get; set; }
    public Reply? Reply { get; set; }
    public Profile? Profile { get; set; }
}