namespace SharedKernel.SeedWorks;

public interface IAuditing
{
    public DateTimeOffset CreatedAt { get; set; }
    public DateTimeOffset? UpdatedAt { get; set; }
}