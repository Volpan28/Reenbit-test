using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ReenbitBooking.Api.ExceptionHandling;

/// <summary>
/// Central place for translating unhandled exceptions into ProblemDetails responses,
/// so MediatR handlers stay free of try-catch blocks.
/// </summary>
public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title, detail) = MapException(exception);

        httpContext.Response.StatusCode = statusCode;

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = title,
                Detail = detail,
                Instance = httpContext.Request.Path
            }
        });
    }

    private (int StatusCode, string Title, string Detail) MapException(Exception exception)
    {
        switch (exception)
        {
            // A concurrent write already changed/removed the row this request tried to update.
            // Surface it as a conflict instead of letting EF's exception bubble up as a 500.
            case DbUpdateConcurrencyException concurrencyException:
                logger.LogWarning(
                    concurrencyException,
                    "Concurrency conflict while updating {EntityCount} entity(ies).",
                    concurrencyException.Entries.Count);

                return (
                    StatusCodes.Status409Conflict,
                    "Concurrency Conflict",
                    "The resource was modified by another request. Please reload the data and try again.");

            default:
                logger.LogError(exception, "Unhandled exception occurred.");

                return (
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred",
                    "An unexpected error occurred while processing your request.");
        }
    }
}
