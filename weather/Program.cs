using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using weather;
using weather.Shared;
using MudBlazor.Services;
using Blazored.LocalStorage;
using System.Net.Http.Json;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var baseHttpClient = new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) };

// Load appsettings.json
var appSettingsService = new AppSettingsService();
await appSettingsService.LoadAsync(baseHttpClient);

// Register services
builder.Services.AddSingleton(appSettingsService);
builder.Services.AddScoped(sp => baseHttpClient);
builder.Services.AddScoped<GlobalState>();
builder.Services.AddAuthorizationCore();
builder.Services.AddMudServices();
builder.Services.AddBlazoredLocalStorage();

// Root components
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

await builder.Build().RunAsync();
