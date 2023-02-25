using Maui.FinancialManager.Services.Contracts;

namespace Maui.FinancialManager.Configuration;

public class ProductionConfiguration : IServiceUris
{
    public ProductionConfiguration()
    {
        this.BaseUri = new Uri("http://api.danielancines.com/");
    }

    public Uri BaseUri { get; }
}

