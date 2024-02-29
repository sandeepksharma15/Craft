using Craft.Security.CurrentUserService;
using Microsoft.AspNetCore.Http;
using Serilog;
using Serilog.Context;

namespace Craft.Infrastructure.Middleware;

public class ResponseLoggingMiddleware(ICurrentUser currentUser) : IMiddleware
{
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        await next(context);

        var originalBody = context.Response.Body;

        await using var newBody = new MemoryStream();

        context.Response.Body = newBody;

        string responseBody;

        if (context.Request.Path.ToString().Contains("tokens"))
            responseBody = "[Redacted] Contains Sensitive Information.";
        else
        {
            newBody.Seek(0, SeekOrigin.Begin);
            responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();
        }

        string email = _currentUser.GetEmail() is string userEmail ? userEmail : "Anonymous";

        var userId = _currentUser.GetId();

        string tenant = _currentUser.GetTenant() ?? string.Empty;

        if (userId != default) LogContext.PushProperty("UserId", userId);

        LogContext.PushProperty("UserEmail", email);

        if (!string.IsNullOrEmpty(tenant)) LogContext.PushProperty("Tenant", tenant);

        LogContext.PushProperty("StatusCode", context.Response.StatusCode);
        LogContext.PushProperty("ResponseTimeUTC", DateTime.UtcNow);

        Log
            .ForContext("ResponseHeaders", context.Response.Headers.ToDictionary(h => h.Key, h => h.Value.ToString()), destructureObjects: true)
            .ForContext("ResponseBody", responseBody)
            .Information("HTTP {RequestMethod} Request to {RequestPath} by {RequesterEmail} has Status Code {StatusCode}.", context.Request.Method, context.Request.Path, string.IsNullOrEmpty(email) ? "Anonymous" : email, context.Response.StatusCode);

        newBody.Seek(0, SeekOrigin.Begin);
        await newBody.CopyToAsync(originalBody);
    }
}
