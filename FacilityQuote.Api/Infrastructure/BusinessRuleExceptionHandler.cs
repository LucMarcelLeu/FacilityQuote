using FacilityQuote.Application.Common;
using Microsoft.AspNetCore.Diagnostics;

namespace FacilityQuote.Api.Infrastructure;

public class BusinessRuleExceptionHandler(
    ILogger<BusinessRuleExceptionHandler> logger)
    : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        if (exception is ResourceNotFoundException)
        {
            httpContext.Response.StatusCode =
                StatusCodes.Status404NotFound;

            await Results.Problem(
                statusCode: StatusCodes.Status404NotFound,
                title: "Resource not found",
                detail: exception.Message)
                .ExecuteAsync(httpContext);

            return true;
        }

        if (exception is BusinessRuleException)
        {
            logger.LogWarning(
                exception,
                "Business rule violation: {Message}",
                exception.Message);

            httpContext.Response.StatusCode =
                StatusCodes.Status400BadRequest;

            await Results.Problem(
                statusCode: StatusCodes.Status400BadRequest,
                title: "Business rule violation",
                detail: exception.Message)
                .ExecuteAsync(httpContext);

            return true;
        }

        return false;
    }
}