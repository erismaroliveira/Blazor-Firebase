using Blazored.LocalStorage;
using Blazorfirebase.Client;
using Blazorfirebase.Client.Authentication;
using Blazorfirebase.Client.Network;
using Blazorfirebase.Client.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Refit;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddBlazoredLocalStorage();

//builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAuthorizationCore();
builder.Services.AddScoped<AuthHandler>();

builder.Services.AddRefitClient<IWeatherApiService>(new RefitSettings(new NewtonsoftJsonContentSerializer()))
    .ConfigureHttpClient(c => c.BaseAddress = new Uri("https://localhost:7182/"))
    .AddHttpMessageHandler<AuthHandler>();

builder.Services.AddScoped<AuthenticationStateProvider, CustomAuthenticationStateProvider>();
builder.Services.AddScoped<IWeatherService, WeatherService>();

await builder.Build().RunAsync();
