using FluentValidation.Results;

namespace SharedKernel.SeedWorks;

public class Result
{
    public bool IsSuccess { get; init; }
    public string? Message { get; init; }
    public ErrorType? Type { get; init; }
    public IDictionary<string, string[]>? Errors { get; init; }
    public static Result Success() => new() { IsSuccess = true };

    public static Result Fail(ErrorType type, string error) =>
        new() { IsSuccess = false, Message = error, Type = type };

    public static Result Fail(ValidationResult validationResult)
    {
        return new Result
        {
            IsSuccess = false,
            Message = "Validation failed",
            Errors = validationResult.ToDictionary(),
            Type = ErrorType.Validation
        };
    }
}

public class Result<T> : Result
{
    public T? Data { get; init; }

    public static Result<T> Success(T data) => new() { IsSuccess = true, Data = data };

    public new static Result<T> Fail(ErrorType type, string error) =>
        new() { IsSuccess = false, Message = error, Type = type };
}