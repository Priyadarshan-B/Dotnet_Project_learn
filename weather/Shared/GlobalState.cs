using System;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Blazored.LocalStorage;

namespace weather.Shared;

public class GlobalState
{
    private readonly ILocalStorageService _localStorage;

    public string UserId { get; private set; } = string.Empty;
    public string UserName { get; private set; } = string.Empty;
    public string Email { get; private set; } = string.Empty;

    public event Action? OnChange;

    public GlobalState(ILocalStorageService localStorage)
    {
        _localStorage = localStorage;
    }

    public async Task LoadUserData()
    {
        var token = await _localStorage.GetItemAsStringAsync("D!");

        
        if (string.IsNullOrEmpty(token))
        {
            Console.WriteLine("No token found in localStorage.");
            return;
        }
        token = token.Trim('"');

        var handler = new JwtSecurityTokenHandler();
        if (!handler.CanReadToken(token))
        {
            Console.WriteLine("The token is not well formed.");
            return;
        }
        try
        {
            var jwtToken = handler.ReadJwtToken(token);

            UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
            UserName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
            Email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;

            //Console.WriteLine($"User Loaded: {UserName}, {Email}");

            NotifyStateChanged();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error reading JWT: {ex.Message}");
        }
    }

    public async Task Logout()
    {
        await _localStorage.RemoveItemAsync("D!");
        UserId = UserName = Email = string.Empty;
        NotifyStateChanged();
    }

    private void NotifyStateChanged() => OnChange?.Invoke();
}
