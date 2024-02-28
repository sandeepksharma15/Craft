﻿using System.Security.Claims;
using Craft.Security.Extensions;

namespace Craft.Security.CurrentUserService;

public class CurrentUser<TKey> : ICurrentUser<TKey>
{
    public CurrentUser(ICurrentUserProvider currentUserProvider)
    {
        _user = currentUserProvider?.GetUser();

        Id = GetId();
    }

    protected ClaimsPrincipal _user;

    public TKey Id { get; set; }

    public string Name => _user?.Identity?.Name;

    public string GetEmail() => IsAuthenticated() ? _user?.GetEmail() : string.Empty;

    public string GetFirstName() => IsAuthenticated() ? _user?.GetFirstName() : string.Empty;

    public string GetFullName() => IsAuthenticated() ? _user?.GetFullName() : string.Empty;

    public TKey GetId()
        => _user?.GetUserId() != null
                ? (TKey)Convert.ChangeType(_user.GetUserId(), typeof(TKey))
                : default;

    public string GetImageUrl() => IsAuthenticated() ? _user?.GetImageUrl() : string.Empty;

    public string GetJwtToken() => IsAuthenticated() ? _user?.GetJwtToken() : string.Empty;

    public string GetLastName() => IsAuthenticated() ? _user?.GetLastName() : string.Empty;

    public string GetPermissions() => IsAuthenticated() ? _user?.GetPermissions() : string.Empty;

    public string GetPhoneNumber() => IsAuthenticated() ? _user?.GetMobileNumber() : string.Empty;

    public string GetRole() => IsAuthenticated() ? _user?.GetRole() : string.Empty;

    public string GetTenant() => IsAuthenticated() ? _user?.GetTenant() : string.Empty;

    public IEnumerable<Claim> GetUserClaims() => _user?.Claims;

    public bool IsAuthenticated() => _user?.Identity?.IsAuthenticated is true;

    public bool IsInRole(string role) => _user?.IsInRole(role) is true;

    public void SetCurrentUserId(TKey id) => Id = id;
}

public class CurrentUser(ICurrentUserProvider currentUserProvider)
    : CurrentUser<KeyType>(currentUserProvider), ICurrentUser
{ }