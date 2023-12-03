namespace Microsoft.AspNetCore.Identity;

public static class UserManagerExtensions
{
    /// <summary>
    /// Confirms the user's phone number using the provided confirmation code.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system.</typeparam>
    /// <typeparam name="TId">The type of the user's unique identifier.</typeparam>
    /// <param name="userManager">The <see cref="UserManager{TUser}"/> to perform the operation.</param>
    /// <param name="id">The unique identifier of the user.</param>
    /// <param name="code">The confirmation code sent to the user's phone number.</param>
    /// <returns>A <see cref="Task{IdentityResult}"/> representing the asynchronous operation's result,
    /// indicating whether the confirmation was successful or not.</returns>
    /// <remarks>
    /// If the user with the specified <paramref name="id"/> does not exist, the method returns a failed <see cref="IdentityResult"/>
    /// with an error indicating that the user does not exist.
    /// </remarks>
    public static async Task<IdentityResult> ConfirmPhoneNumberAsync<TUser, TId>(this UserManager<TUser> userManager,
        TId id, string code)
        where TUser : IdentityUser<TId>
        where TId : IEquatable<TId>
    {
        var user = await userManager.FindByIdAsync(id.ToString());

        if (user is null)
            return IdentityResult.Failed([new IdentityError { Code = "", Description = $"User with Id [{id}] does not exist" }]);

        return await userManager.ChangePhoneNumberAsync(user, user.PhoneNumber, code);
    }

    /// <summary>
    /// Asynchronously retrieves a list of role names for the specified user.
    /// </summary>
    /// <typeparam name="TUser">The type representing a user in the system.</typeparam>
    /// <typeparam name="TId">The type of the user's unique identifier.</typeparam>
    /// <param name="userManager">The <see cref="UserManager{TUser}"/> to perform the operation.</param>
    /// <param name="id">The unique identifier of the user.</param>
    /// <returns>
    /// A <see cref="Task{IList{string}}"/> representing the asynchronous operation's result, containing a list of role names
    /// to which the user belongs. If the user does not exist, an empty list is returned.
    /// </returns>
    public static async Task<IList<string>> GetRolesAsync<TUser, TId>(this UserManager<TUser> userManager, TId id)
        where TUser : IdentityUser<TId>
        where TId : IEquatable<TId>
    {
        TUser user = await userManager.FindByIdAsync(id.ToString());

        if (user is not null)
            return [.. (await userManager.GetRolesAsync(user))];

        return [];
    }
}
