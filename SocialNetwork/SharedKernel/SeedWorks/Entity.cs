using System.ComponentModel.DataAnnotations;

namespace SharedKernel.SeedWorks;

public abstract class Entity<TId> : EventsBase
{
    [Key] public TId? Id { get; set; }
}