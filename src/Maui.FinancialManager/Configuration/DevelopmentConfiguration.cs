using Maui.FinancialManager.Services.Contracts;

namespace Maui.FinancialManager.Configuration;

public class DevelopmentConfiguration : IServiceUris
{
    public DevelopmentConfiguration()
    {
        this.BaseUri = new Uri("http://localhost:5095/");
    }

    public Uri BaseUri { get; }
}

