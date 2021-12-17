using GeekBurger.StoreCatalog.Client.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GeekBurger.Ingredients.Contract.DTO;
using System.Linq;

namespace GeekBurger.StoreCatalog.Client
{
    public class IngredientsClient : ClientHttp, IIgredients
    {
        private readonly IProduction _production;

        public IngredientsClient(IProduction production)
        {
            _production = production;
        }
        async Task<IngredientsResponse> IIgredients.GetByRestrictions(IngredientsRequest ingredients)
        {
            IngredientsResponse response = null;
            try
            {

                HttpContent content = new StringContent(JsonSerializer.Serialize(ingredients), System.Text.Encoding.UTF8, "application/json"); ;
                HttpResponseMessage responseJson = await clientHttp.PostAsync("https://geekburgeringredients20211216191440.azurewebsites.net/", content);

                responseJson.EnsureSuccessStatusCode();
                string responseBody = await responseJson.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<IngredientsResponse>(responseBody);


                //TODO:Analisar  ProductId
                //
                //var AreaFrom = _production.GetAreas().Result.Where(x=>x.ProductionId == response.ProductId)
                //TODO:realizar publish das areas 



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
