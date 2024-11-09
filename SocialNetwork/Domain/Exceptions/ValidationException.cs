using FluentValidation.Results;

namespace Domain.Exceptions;

public class ValidationException(ValidationResult validationResult) : Exception
{
    public IDictionary<string, string[]>? Errors { get; } = validationResult.ToDictionary();
}