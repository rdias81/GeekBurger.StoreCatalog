using GeekBurger.StoreCatalog.Client.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Contract.Request;
using System.Collections.Generic;

namespace GeekBurger.StoreCatalog.Client
{
    public class ProductsClient : ClientHttp, IProducts
    {
        async Task<List<ProductToGet>> IProducts.GetProducts(string storeName)
        {
            try
            {
                HttpResponseMessage responseJson = await clientHttp.GetAsync($"https://geekburger-products.azurewebsites.net/api/products?storeName={storeName}");
                responseJson.EnsureSuccessStatusCode();

                var responseBody = await responseJson.Content.ReadAsStreamAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };

                return await JsonSerializer.DeserializeAsync<List<ProductToGet>>(responseBody, options);
            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException!");
                Console.WriteLine("Erro :{0} ", e.Message);
                throw;
            }

        }

    }
}
