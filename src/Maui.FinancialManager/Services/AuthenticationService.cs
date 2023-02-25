using System;
using System.Text;
using System.Text.Json;
using Maui.FinancialManager.Services.Base;
using Maui.FinancialManager.Services.Contracts;

namespace Maui.FinancialManager.Services;

public class AuthenticationService : BaseService
{
    private readonly IServiceUris _serviceUris;

    public AuthenticationService(IServiceUris serviceUris)
    {
        this._serviceUris = serviceUris;
    }

    public async Task<string> Authenticate(string login, string password)
    {
        var data = new
        {
            login = login,
            password = password
        };

        var body = new StringContent(JsonSerializer.Serialize<dynamic>(data),
                                        Encoding.UTF8, "application/json");

        var client = this.GetClient(this._serviceUris.BaseUri);

        var response = await client.PostAsync("api/v1/auth", body);
        if (response.IsSuccessStatusCode)
        {
            var resultElement = JsonSerializer.Deserialize<JsonElement>(await response.Content.ReadAsStringAsync());
            return resultElement.GetProperty("token").GetString();
        }
        else
        {
            return string.Empty;
        }
    }
}
