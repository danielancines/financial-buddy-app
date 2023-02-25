using System;
namespace Maui.FinancialManager.Services.Base;

public abstract class BaseService
{
    public HttpClient GetClient(Uri baseAddress)
    {
        var client = new HttpClient();
        client.BaseAddress = baseAddress;
        client.DefaultRequestHeaders.Add("authorization", $"Bearer {Preferences.Get("Token", string.Empty)}");
        return client;
    }
}

