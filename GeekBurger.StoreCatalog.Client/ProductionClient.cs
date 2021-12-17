using GeekBurger.StoreCatalog.Client.Interfaces;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Client
{
    public class ProductionClient : ClientHttp, IProduction
    {
        async Task<dynamic> IProduction.GetAreas()
        {
            try
            {
                HttpResponseMessage response = await clientHttp.GetAsync("http://www.contoso.com/");
                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);

                return JsonConvert.DeserializeObject<Production.Contract.Areas>(responseBody);
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
