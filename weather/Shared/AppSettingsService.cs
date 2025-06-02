using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

public class AppSettingsService
{
    public string ApiBaseUrl { get; private set; } = "";

    public async Task LoadAsync(HttpClient http)
    {
        var config = await http.GetFromJsonAsync<AppSettings>("appsettings.json");
        ApiBaseUrl = config?.ApiBaseUrl ?? "";
    }
}
