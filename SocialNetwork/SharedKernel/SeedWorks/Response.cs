namespace SharedKernel.SeedWorks;

public record Response<T>(
    T? Data,
    string Message,
    bool Succeeded = true);

public record Response(
    string Message,
    bool Succeeded = true);