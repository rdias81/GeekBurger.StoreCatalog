using GeekBurger.Production.Contract;
using GeekBurger.StoreCatalog.Client.Interfaces;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Client
{
    public class ProductionClient : ClientHttp, IProduction
    {
        async Task<List<Areas>> IProduction.GetAreas()
        {
            try
            {
                HttpResponseMessage response = await clientHttp.GetAsync("https://geekburgerproductiongrupo4.azurewebsites.net/areas");
                response.EnsureSuccessStatusCode();

                var responseBody = await response.Content.ReadAsStreamAsync();

                var options = new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                };
                
                return await JsonSerializer.DeserializeAsync<List<Areas>>(responseBody, options);
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
