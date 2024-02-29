using System.Net;
using Craft.Domain.Exceptions;
using Craft.Domain.Helpers;
using Microsoft.AspNetCore.Http;
using Serilog;

namespace Craft.Infrastructure.Middleware;

public class ExceptionMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next(context);
        }
        catch (Exception exception)
        {
            // TODO: Capture Details Like UserId, TenantId

            var errorId = Guid.NewGuid().ToString();

            var serverResponse = new ServerResponse
            {
                ErrorId = errorId,
                Source = exception.TargetSite?.DeclaringType?.FullName,
                Message = exception.Message.Trim(),
                SupportMessage = $"Provide the ErrorId {errorId} to the support team for further analysis."
            };

            serverResponse.Errors.Add(exception.Message);

            if (exception is not CustomException && exception.InnerException != null)
                while (exception.InnerException != null)
                    exception = exception.InnerException;

            switch (exception)
            {
                case CustomException e:
                    serverResponse.StatusCode = (int)e.StatusCode;
                    if (e.Errors is not null)
                        serverResponse.Errors.AddIfNotContains(e.Errors);
                    break;

                case KeyNotFoundException:
                    serverResponse.StatusCode = (int)HttpStatusCode.NotFound;
                    break;

                default:
                    serverResponse.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            Log.Error($"""
                {serverResponse.Message} Request failed with Status Code {context.Response.StatusCode}
                    and Error Id {errorId}.
                """
                );

            var response = context.Response;

            if (!response.HasStarted)
            {
                response.ContentType = "application/json";
                response.StatusCode = serverResponse.StatusCode;
                await response.WriteAsJsonAsync(serverResponse);
            }
            else
                Log.Warning("Can't write error response. Response has already started.");
        }
    }
}
