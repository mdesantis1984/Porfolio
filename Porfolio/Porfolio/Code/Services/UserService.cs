using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Porfolio.Code.Services;


public class UserService
{
    private readonly AuthenticationStateProvider _authenticationStateProvider;

    public UserService(AuthenticationStateProvider authenticationStateProvider)
    {
        _authenticationStateProvider = authenticationStateProvider;
    }

    public async Task<ClaimsPrincipal> GetUserAsync()
    {
        var authState = await _authenticationStateProvider.GetAuthenticationStateAsync();
        return authState.User;
    }

    public async Task<string> GetUserNameAsync()
    {
        var user = await GetUserAsync();
        return user.Identity?.Name;
    }

    public async Task<string> GetUserEmailAsync()
    {
        var user = await GetUserAsync();
        return user.FindFirst(ClaimTypes.Email)?.Value;
    }
}
