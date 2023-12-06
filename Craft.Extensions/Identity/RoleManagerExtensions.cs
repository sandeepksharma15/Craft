using System.Security.Claims;

namespace Microsoft.AspNetCore.Identity;

public static class RoleManagerExtensions
{
    /// <summary>
    /// Asynchronously adds a role to the role store.
    /// </summary>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <param name="roleManager">The RoleManager to use for role management.</param>
    /// <param name="entity">The role to be added.</param>
    /// <returns>The added role.</returns>
    public static async Task<TRole> AddAsync<TRole>(this RoleManager<TRole> roleManager, TRole entity)
        where TRole : class
    {
        _ = await roleManager.CreateAsync(entity);

        return entity;
    }

    /// <summary>
    /// Asynchronously creates a role claim and adds it to the specified role.
    /// </summary>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TId">The type of the role's primary key.</typeparam>
    /// <param name="roleManager">The RoleManager to use for role management.</param>
    /// <param name="roleClaim">The role claim to be created and added.</param>
    /// <returns>The result of the asynchronous operation.</returns>
    public static async Task<IdentityResult> CreateRoleClaimAsync<TRole, TId>(this RoleManager<TRole> roleManager, IdentityRoleClaim<TId> roleClaim)
        where TRole : class
        where TId : IEquatable<TId>
    {
        var appRole = await roleManager.FindByIdAsync(roleClaim.RoleId.ToString());

        if (appRole is null)
            return IdentityResult.Failed([new IdentityError { Code = "", Description = $"Id [{roleClaim.RoleId}] does not exist" }]);

        return await roleManager
                .AddClaimAsync(appRole, new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
    }

    /// <summary>
    /// Asynchronously retrieves a specific claim associated with the specified role.
    /// </summary>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <param name="roleManager">The RoleManager to use for role management.</param>
    /// <param name="role">The role to retrieve the claim from.</param>
    /// <param name="claimType">The type of the claim to retrieve.</param>
    /// <returns>The claim associated with the specified role and claim type, or null if not found.</returns>
    public static async Task<Claim> GetRoleClaimAsync<TRole>(this RoleManager<TRole> roleManager, TRole role, string claimType)
        where TRole : class
    {
        var claims = await roleManager.GetClaimsAsync(role);

        if (claims.Count == 0 && claims.SingleOrDefault(x => x.Type == claimType) == default)
            return null;

        return claims.SingleOrDefault(x => x.Type == claimType);
    }

    /// <summary>
    /// Asynchronously retrieves a specific claim associated with a role identified by its name.
    /// </summary>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <param name="roleManager">The RoleManager to use for role management.</param>
    /// <param name="roleName">The name of the role to retrieve the claim from.</param>
    /// <param name="claimType">The type of the claim to retrieve.</param>
    /// <returns>The claim associated with the specified role name and claim type, or null if not found.</returns>
    public static async Task<Claim> GetRoleClaimAsync<TRole>(this RoleManager<TRole> roleManager, string roleName, string claimType)
        where TRole : class
    {
        var role = await roleManager.FindByNameAsync(roleName);

        if (role is null) return null;

        return await roleManager.GetRoleClaimAsync<TRole>(role, claimType);
    }

    /// <summary>
    /// Asynchronously retrieves a specific claim associated with a role identified by its ID.
    /// </summary>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TId">The type of the role's primary key.</typeparam>
    /// <param name="roleManager">The RoleManager to use for role management.</param>
    /// <param name="id">The ID of the role to retrieve the claim from.</param>
    /// <param name="claimType">The type of the claim to retrieve.</param>
    /// <returns>
    /// The claim associated with the specified role ID and claim type, or null if the role is not found.
    /// </returns>
    public static async Task<Claim?> GetRoleClaimAsync<TRole, TId>(this RoleManager<TRole> roleManager, TId id, string claimType)
        where TRole : class
        where TId : IEquatable<TId>
    {
        var appRole = await roleManager.FindByIdAsync(id.ToString());

        if (appRole is null) return null;

        return await roleManager.GetRoleClaimAsync(appRole, claimType);
    }

    /// <summary>
    /// Asynchronously retrieves the value of a specific claim associated with a role.
    /// </summary>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TId">The type of the role's primary key.</typeparam>
    /// <param name="roleManager">The RoleManager to use for role management.</param>
    /// <param name="role">The role to retrieve the claim value from.</param>
    /// <param name="claimType">The type of the claim to retrieve.</param>
    /// <returns>
    /// The value of the claim associated with the specified role and claim type, or null if the claim is not found.
    /// </returns>
    public static async Task<string> GetRoleClaimValueAsync<TRole>(this RoleManager<TRole> roleManager, TRole role, string claimType)
        where TRole : class
    {
        var claims = await roleManager.GetClaimsAsync(role);

        if (claims.Count == 0 && claims.SingleOrDefault(x => x.Type == claimType) == default)
            return null;

        return claims.SingleOrDefault(x => x.Type == claimType).Value;
    }

    /// <summary>
    /// Asynchronously retrieves the value of a specific claim associated with a role identified by its name.
    /// </summary>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <param name="roleManager">The RoleManager to use for role management.</param>
    /// <param name="roleName">The name of the role to retrieve the claim value from.</param>
    /// <param name="claimType">The type of the claim to retrieve.</param>
    /// <returns>
    /// The value of the claim associated with the specified role name and claim type, or null if the role is not found.
    /// </returns>
    public static async Task<string?> GetRoleClaimValueAsync<TRole>(this RoleManager<TRole> roleManager, string roleName, string claimType)
        where TRole : class
    {
        var role = await roleManager.FindByNameAsync(roleName);

        if (role is null) return null;

        return await roleManager.GetRoleClaimValueAsync<TRole>(role, claimType);
    }

    /// <summary>
    /// Asynchronously retrieves the value of a specific claim associated with a role identified by its Id.
    /// </summary>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <param name="roleManager">The RoleManager to use for role management.</param>
    /// <param name="roleId">The Id of the role to retrieve the claim value from.</param>
    /// <param name="claimType">The type of the claim to retrieve.</param>
    /// <returns>
    /// The value of the claim associated with the specified role name and claim type, or null if the role is not found.
    /// </returns>
    public static async Task<string?> GetRoleClaimValueAsync<TRole, TId>(this RoleManager<TRole> roleManager, TId roleId, string claimType)
        where TRole : class
    {
        var role = await roleManager.FindByIdAsync(roleId.ToString());

        if (role is null) return null;

        return await roleManager.GetRoleClaimValueAsync<TRole>(role, claimType);
    }

    /// <summary>
    /// Asynchronously removes a specific claim associated with a role identified by its ID.
    /// </summary>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TId">The type of the role's primary key.</typeparam>
    /// <param name="roleManager">The RoleManager to use for role management.</param>
    /// <param name="roleClaim">The role claim to remove.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the IdentityResult of the removal operation.
    /// </returns>
    public static async Task<IdentityResult> RemoveRoleClaimAsync<TRole, TId>(this RoleManager<TRole> roleManager, IdentityRoleClaim<TId> roleClaim)
        where TRole : class
        where TId : IEquatable<TId>
    {
        var appRole = await roleManager.FindByIdAsync(roleClaim.RoleId.ToString());

        if (appRole is null)
            return IdentityResult.Failed([new IdentityError { Code = "", Description = $"Id [{roleClaim.RoleId}] does not exist" }]);

        IList<Claim> claims = await roleManager.GetClaimsAsync(appRole);

        if (claims.Count == 0)
            return IdentityResult.Failed([new IdentityError { Code = "", Description = $"Id [{roleClaim.RoleId}] does not have any claim" }]);

        if (claims.SingleOrDefault(x => x.Type == roleClaim.ClaimType) == default)
            return IdentityResult.Failed([new IdentityError { Code = "", Description = $"Id [{roleClaim.RoleId}] does not have claim [{roleClaim.ClaimType}]" }]);

        return await roleManager
            .RemoveClaimAsync(appRole, claims.SingleOrDefault(x => x.Type == roleClaim.ClaimType));
    }

    /// <summary>
    /// Asynchronously updates a specific claim for a role identified by its ID.
    /// </summary>
    /// <typeparam name="TRole">The type representing a role.</typeparam>
    /// <typeparam name="TId">The type of the role's primary key.</typeparam>
    /// <param name="roleManager">The RoleManager to use for role management.</param>
    /// <param name="roleClaim">The role claim containing the updated information.</param>
    /// <returns>
    /// A task that represents the asynchronous operation, containing the IdentityResult of the update operation.
    /// </returns>
    public static async Task<IdentityResult> UpdateRoleClaimAsync<TRole, TId>(this RoleManager<TRole> roleManager, IdentityRoleClaim<TId> roleClaim)
        where TRole : class
        where TId : IEquatable<TId>
    {
        var appRole = await roleManager.FindByIdAsync(roleClaim.RoleId.ToString());

        if (appRole is null)
            return IdentityResult.Failed([new IdentityError { Code = "", Description = $"Id [{roleClaim.RoleId}] does not exist" }]);

        IList<Claim>? claims = await roleManager.GetClaimsAsync(appRole);

        if (claims.Count == 0)
            return IdentityResult.Failed([new IdentityError { Code = "", Description = $"Id [{roleClaim.RoleId}] does not have any claim" }]);

        if (claims.SingleOrDefault(x => x.Type == roleClaim.ClaimType) == default)
            return IdentityResult.Failed([new IdentityError { Code = "", Description = $"Id [{roleClaim.RoleId}] does not have claim [{roleClaim.ClaimType}]" }]);

        await roleManager
            .RemoveClaimAsync(appRole, claims.SingleOrDefault(x => x.Type == roleClaim.ClaimType));

        return await roleManager
            .AddClaimAsync(appRole, new Claim(roleClaim.ClaimType, roleClaim.ClaimValue));
    }
}
