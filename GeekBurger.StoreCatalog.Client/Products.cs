using GeekBurger.StoreCatalog.Client.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GeekBurger.Products.Contract;

namespace GeekBurger.StoreCatalog.Client
{
    public class Products : ClientHttp, IProducts
    {
        async Task<dynamic> IProducts.GetProducts(ProductToGet  products)
        {
            //GeekBurger.Products.Contract.
                dynamic response = null;
                try
                {

                    HttpContent content = new StringContent(JsonSerializer.Serialize(products), System.Text.Encoding.UTF8, "application/json"); ;
                    HttpResponseMessage responseJson = await clientHttp.PostAsync("https://geekburger-products.azurewebsites.net", content);

                    responseJson.EnsureSuccessStatusCode();
                    string responseBody = await responseJson.Content.ReadAsStringAsync();
                    response = JsonSerializer.Deserialize<dynamic>(responseBody);
                    Console.WriteLine(response);
                    return response;

                }
                catch (HttpRequestException e)
                {
                    Console.WriteLine("\nException!");
                    Console.WriteLine("Erro :{0} ", e.Message);
                    return response;
                }
            
        }

    }
}
