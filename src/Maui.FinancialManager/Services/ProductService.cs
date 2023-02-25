using System;
using System.Text.Json;
using Maui.FinancialManager.Models;
using Maui.FinancialManager.Services.Base;
using Maui.FinancialManager.Services.Contracts;

namespace Maui.FinancialManager.Services;

public class ProductsService : BaseService
{
    private readonly IServiceUris _serviceUris;

    public ProductsService(IServiceUris serviceUris)
    {
        this._serviceUris = serviceUris;
    }

    public async Task<IEnumerable<Product>> Get()
    {
        var products = new List<Product>();
        var httpClient = this.GetClient(this._serviceUris.BaseUri);
        var response = await httpClient.GetAsync("api/v1/product");

        if (response.IsSuccessStatusCode)
        {
            products = JsonSerializer.Deserialize<List<Product>>(await response.Content.ReadAsStringAsync());
        }

        return products;
    }
}

