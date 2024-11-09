namespace SharedKernel.SeedWorks;

public record ErrorResponse(
    string Message,
    IDictionary<string, string[]>? Errors = null,
    bool Succeeded = false);