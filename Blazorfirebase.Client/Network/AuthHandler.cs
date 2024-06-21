using Blazored.LocalStorage;
using System.Net.Http.Headers;

namespace Blazorfirebase.Client.Network;

public class AuthHandler : DelegatingHandler
{
    private ILocalStorageService _localStorageService;

    public AuthHandler(ILocalStorageService localStorageService)
    {
        _localStorageService = localStorageService;
    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _localStorageService.GetItemAsync<string>("tokenJWT");

        request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        return await base.SendAsync(request, cancellationToken).ConfigureAwait(false);
    }
}
