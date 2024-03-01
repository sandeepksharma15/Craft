using System.Net.Http.Headers;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Logging.Abstractions;
using Microsoft.Extensions.Primitives;

namespace Craft.Jobs.Jobs;

public class HangfireAuthenticationFilter(ILogger logger) : IDashboardAuthorizationFilter
{
    private const string _AuthenticationScheme = "Basic";
    private readonly ILogger _logger = logger;

    public string Pass { get; set; } = default!;
    public string User { get; set; } = default!;

    public HangfireAuthenticationFilter() : this(new NullLogger<HangfireAuthenticationFilter>())
    {
    }

    private static BasicAuthenticationTokens ExtractAuthenticationTokens(AuthenticationHeaderValue authValues)
    {
        string parameter = System.Text.Encoding.UTF8.GetString(Convert.FromBase64String(authValues.Parameter!));
        string[] parts = parameter.Split(':');
        return new BasicAuthenticationTokens(parts);
    }

    private static bool MissingAuthorizationHeader(StringValues header) => string.IsNullOrWhiteSpace(header);

    private static bool NotBasicAuthentication(AuthenticationHeaderValue authValues)
        => !_AuthenticationScheme.Equals(authValues.Scheme, StringComparison.InvariantCultureIgnoreCase);

    private static void SetChallengeResponse(HttpContext httpContext)
    {
        httpContext.Response.StatusCode = 401;
        httpContext.Response.Headers.Append("WWW-Authenticate", "Basic realm=\"Hangfire Dashboard\"");
    }

    public bool Authorize(DashboardContext context)
    {
        var httpContext = context.GetHttpContext();
        var header = httpContext.Request.Headers.Authorization;

        if (MissingAuthorizationHeader(header))
        {
            _logger.LogInformation("Request is missing Authorization Header");
            SetChallengeResponse(httpContext);
            return false;
        }

        var authValues = AuthenticationHeaderValue.Parse(header);

        if (NotBasicAuthentication(authValues))
        {
            _logger.LogInformation("Request is NOT BASIC authentication");
            SetChallengeResponse(httpContext);
            return false;
        }

        var tokens = ExtractAuthenticationTokens(authValues);

        if (tokens.AreInvalid())
        {
            _logger.LogInformation("Authentication tokens are invalid (empty, null, whitespace)");
            SetChallengeResponse(httpContext);
            return false;
        }

        if (tokens.CredentialsMatch(User, Pass))
        {
            _logger.LogInformation("Awesome, authentication tokens match configuration!");
            return true;
        }

        _logger.LogInformation($"Boo! Authentication tokens [{tokens.Username}] [{tokens.Password}] do not match configuration");

        SetChallengeResponse(httpContext);
        return false;
    }
}

public class BasicAuthenticationTokens(string[] tokens)
{
    private readonly string[] _tokens = tokens;

    public string Password => _tokens[1];
    public string Username => _tokens[0];

    private static bool ValidTokenValue(string token)
        => string.IsNullOrWhiteSpace(token);

    private bool ContainsTwoTokens()
        => _tokens.Length == 2;

    public bool AreInvalid()
                => ContainsTwoTokens() && ValidTokenValue(Username) && ValidTokenValue(Password);

    public bool CredentialsMatch(string user, string pass)
        => Username.Equals(user) && Password.Equals(pass);
}
