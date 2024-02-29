using System.Net;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.AspNetCore.Mvc;
using Craft.Domain.Exceptions;

namespace Craft.Infrastructure.Middleware;

public class GlobalExceptionHandler(ILogger<GlobalExceptionHandler> logger) : IExceptionHandler
{
    private readonly ILogger<GlobalExceptionHandler> _logger = logger;

    public async ValueTask<bool> TryHandleAsync(HttpContext context, Exception exception, CancellationToken cancellationToken)
    {
        // TODO: Capture Details Like UserId, TenantId

        var errorId = Guid.NewGuid().ToString();

        var problemDetails = new ProblemDetails
        {
            Instance = errorId,
            Detail = exception.Message.Trim(),
            Type = "https://datatracker.ietf.org/doc/html/rfc7231#section-6.6.1",
        };

        if (exception is not CustomException && exception.InnerException != null)
            while (exception.InnerException != null)
                exception = exception.InnerException;

        problemDetails.Status = exception switch
        {
            CustomException e => (int)e.StatusCode,
            KeyNotFoundException => (int)HttpStatusCode.NotFound,
            _ => (int?)(int)HttpStatusCode.InternalServerError,
        };

        _logger.LogError($"""
                {problemDetails.Detail} Request failed with Status Code {context.Response.StatusCode}
                    and Error Id {errorId}.
                """
            );

        var response = context.Response;

        if (!response.HasStarted)
        {
            response.ContentType = "application/json";
            response.StatusCode = problemDetails.Status.Value;
            await response.WriteAsJsonAsync(problemDetails, cancellationToken);
        }
        else
            Log.Warning("Can't write error response. Response has already started.");

        return true;
    }
}
