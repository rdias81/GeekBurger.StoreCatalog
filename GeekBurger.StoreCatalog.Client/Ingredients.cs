using GeekBurger.StoreCatalog.Client.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Client
{
    public class Ingredients : ClientHttp, IIgredients
    {
        async Task<dynamic> IIgredients.GetByRestrictions(Entities.Ingredients ingredients)
        {
            try
            {
                HttpContent content = new StringContent(JsonSerializer.Serialize(ingredients), System.Text.Encoding.UTF8, "application/json"); ;
                HttpResponseMessage response = await clientHttp.PostAsync("http://www.contoso.com/", content);

                response.EnsureSuccessStatusCode();
                string responseBody = await response.Content.ReadAsStringAsync();
                Console.WriteLine(responseBody);
                return responseBody;

            }
            catch (HttpRequestException e)
            {
                Console.WriteLine("\nException!");
                Console.WriteLine("Erro :{0} ", e.Message);
                return e.Message;
            }
        }

    }
}
