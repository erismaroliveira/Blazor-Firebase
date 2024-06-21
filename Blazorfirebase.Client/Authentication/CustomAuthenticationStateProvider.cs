using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Blazorfirebase.Client.Authentication;

public class CustomAuthenticationStateProvider : AuthenticationStateProvider
{
    private ILocalStorageService _localStorageService;

    public CustomAuthenticationStateProvider(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var identity = new ClaimsIdentity();

        var token = await _localStorageService.GetItemAsync<string>("tokenJWT");

        if (!string.IsNullOrEmpty(token))
            identity = new ClaimsIdentity(ParseJWT(token));

        return await Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)));
    }

    public async Task MarsUserAsLoggedIn(string token)
    {
        var identity = new ClaimsIdentity();
        await _localStorageService.SetItemAsync<string>("tokenJWT", token);

        if (!string.IsNullOrEmpty(token))
            identity = new ClaimsIdentity(ParseJWT(token));

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)))
        );
    }

    public async Task MarsUserAsLoggedOut()
    {
        var identity = new ClaimsIdentity();
        await _localStorageService.RemoveItemAsync("tokenJWT");

        NotifyAuthenticationStateChanged(
            Task.FromResult(new AuthenticationState(new ClaimsPrincipal(identity)))
        );
    }

    public List<Claim> ParseJWT(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var tokenClaims = handler.ReadJwtToken(token);
        return tokenClaims.Claims.ToList();
    }
}
