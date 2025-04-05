// using System;
// using System.IdentityModel.Tokens.Jwt;
// using System.Linq;
// using System.Security.Claims;
// using System.Threading.Tasks;
// using Microsoft.AspNetCore.Components.Authorization;
// using Blazored.LocalStorage;
// using Microsoft.IdentityModel.Tokens;


// namespace weather.Shared
// {
//     public class CustomAuthStateProvider : AuthenticationStateProvider
//     {
//         private readonly ILocalStorageService _localStorage;
//         private readonly GlobalState _globalState;

//         public CustomAuthStateProvider(ILocalStorageService localStorage, GlobalState globalState)
//         {
//             _localStorage = localStorage;
//             _globalState = globalState;
//         }

//         public override async Task<AuthenticationState> GetAuthenticationStateAsync()
//         {
//             var token = await _localStorage.GetItemAsStringAsync("D!");
//             var identity = new ClaimsIdentity();

//            if (!string.IsNullOrEmpty(token))
//     {
//         token = token.Trim('"');
//         var handler = new JwtSecurityTokenHandler();
        
//         try
//         {
//             var jwtToken = handler.ReadJwtToken(token);

//             var expClaim = jwtToken.Claims.FirstOrDefault(c => c.Type == "exp")?.Value;
//             if (long.TryParse(expClaim, out var expTime))
//             {
//                 var expiryDate = DateTimeOffset.FromUnixTimeSeconds(expTime);
//                 if (expiryDate < DateTimeOffset.UtcNow)
//                 {
//                     await MarkUserAsLoggedOut();
//                     return new AuthenticationState(new ClaimsPrincipal()); 
//                 }
//             }

//             identity = new ClaimsIdentity(jwtToken.Claims, "jwt");
//             _globalState.UserId = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value ?? string.Empty;
//             _globalState.UserName = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value ?? string.Empty;
//             _globalState.Email = jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value ?? string.Empty;
//             _globalState.NotifyStateChanged();
//         }
//         catch (SecurityTokenException ex)
//         {
//             Console.WriteLine($"Error decoding JWT: {ex.Message}");
//             await MarkUserAsLoggedOut(); 
//         }
//     }
//             var user = new ClaimsPrincipal(identity);
//             return new AuthenticationState(user);
//         }

//         public async Task MarkUserAsAuthenticated(string token)
//         {
//             await _localStorage.SetItemAsStringAsync("D!", token);
//             NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//         }

//         public async Task MarkUserAsLoggedOut()
//         {
//             await _localStorage.RemoveItemAsync("D!");

//             _globalState.UserId = _globalState.UserName = _globalState.Email = string.Empty;
//             _globalState.NotifyStateChanged();

//             NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
//         }

        
//     }
// }
