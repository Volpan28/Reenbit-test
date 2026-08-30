using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReenbitBooking.Application.Common.Exceptions;
using FluentValidationException = FluentValidation.ValidationException;

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
        var (statusCode, title, detail, extensions) = MapException(exception);

        httpContext.Response.StatusCode = statusCode;

        var problemDetails = new ProblemDetails
        {
            Status = statusCode,
            Title = title,
            Detail = detail,
            Instance = httpContext.Request.Path
        };

        if (extensions is not null)
        {
            foreach (var (key, value) in extensions)
            {
                problemDetails.Extensions[key] = value;
            }
        }

        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = problemDetails
        });
    }

    private (int StatusCode, string Title, string Detail, IDictionary<string, object?>? Extensions) MapException(
        Exception exception)
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
                    "The resource was modified by another request. Please reload the data and try again.",
                    null);

            case NotFoundException notFoundException:
                logger.LogWarning(notFoundException, "Requested entity was not found.");

                return (
                    StatusCodes.Status404NotFound,
                    "Not Found",
                    notFoundException.Message,
                    null);

            case BadRequestException badRequestException:
                logger.LogWarning(badRequestException, "Bad request.");

                return (
                    StatusCodes.Status400BadRequest,
                    "Bad Request",
                    badRequestException.Message,
                    null);

            case FluentValidationException validationException:
                logger.LogWarning(validationException, "Validation failed.");

                var errors = validationException.Errors
                    .GroupBy(failure => failure.PropertyName)
                    .ToDictionary(
                        group => group.Key,
                        group => (object?)group.Select(failure => failure.ErrorMessage).ToArray());

                return (
                    StatusCodes.Status400BadRequest,
                    "Validation Error",
                    "One or more validation errors occurred.",
                    new Dictionary<string, object?> { ["errors"] = errors });

            default:
                logger.LogError(exception, "Unhandled exception occurred.");

                return (
                    StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred",
                    "An unexpected error occurred while processing your request.",
                    null);
        }
    }
}
