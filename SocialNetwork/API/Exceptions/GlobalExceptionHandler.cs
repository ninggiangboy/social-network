using System.Text.Json;
using System.Text.Json.Serialization;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using SharedKernel.SeedWorks;

namespace API.Exceptions;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly JsonSerializerOptions _jsonOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
    };

    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        // logger.LogError(
        //     exception, "Exception occurred: {Message}", exception.Message);

        var (message, errors, code) = exception switch
        {
            ValidationException validationException => (
                "Validation error occurred.",
                validationException.Errors,
                StatusCodes.Status400BadRequest),
            NotFoundException nfe => (
                nfe.Message,
                null,
                StatusCodes.Status404NotFound),
            ForbiddenException => (
                "You are not authorized to perform this action.",
                null,
                StatusCodes.Status403Forbidden),
            UnauthorizedAccessException uae => (
                uae.Message,
                null,
                StatusCodes.Status401Unauthorized),
            ConflictException ce => (
                ce.Message,
                null,
                StatusCodes.Status409Conflict),
            BadRequestException bre => (
                bre.Message,
                null,
                StatusCodes.Status400BadRequest),
            _ => (
                "An error occurred while processing your request.",
                null,
                StatusCodes.Status500InternalServerError)
        };
        var response = new ErrorResponse(message, errors);

        httpContext.Response.StatusCode = code;

        await httpContext.Response
            .WriteAsJsonAsync(response, _jsonOptions, cancellationToken);

        return true;
    }
}