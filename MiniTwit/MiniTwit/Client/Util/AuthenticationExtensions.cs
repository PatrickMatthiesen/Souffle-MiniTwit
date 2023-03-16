using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace MiniTwit.Client.Util;

public static class AuthenticationExtensions
{
    public static async Task<string> GetUserIdAsync(this AuthenticationStateProvider authStateProvider)
    {
        return await _GetUserId(authStateProvider);
    }

    public static async Task<ClaimsPrincipal> GetUserAsync(this AuthenticationStateProvider authStateProvider)
    {
        return await _GetUser(authStateProvider);
    }

    public static async Task<string> GetUserNameAsync(this AuthenticationStateProvider authStateProvider)
    {
        return await _GetUserName(authStateProvider);
    }

    private static async Task<string> _GetUserName(AuthenticationStateProvider authStateProvider)
    {
        var user = await _GetUser(authStateProvider);
        return user.Identity.Name;
    }

    private static async Task<string> _GetUserId(AuthenticationStateProvider authStateProvider)
    {
        var user = await _GetUser(authStateProvider);
        return user.FindFirst(c => c.Type == "sub").Value;
    }

    private static async Task<ClaimsPrincipal> _GetUser(AuthenticationStateProvider authStateProvider)
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();
        return authState.User;
    }
}
