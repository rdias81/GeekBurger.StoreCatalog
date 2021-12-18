using GeekBurger.StoreCatalog.Client.Interfaces;
using System;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using GeekBurger.Ingredients.Contract.DTO;
using System.Linq;
using System.Collections.Generic;
using GeekBurger.Production.Contract;
using GeekBurger.Products.Contract;

namespace GeekBurger.StoreCatalog.Client
{
    public class IngredientsClient : ClientHttp, IIgredients
    {
        private readonly IProduction _production;
        private readonly IProducts _products;
        public IngredientsClient(IProduction production, IProducts products)
        {
            _production = production;
            _products = products;
        }
        async Task<List<IngredientsResponse>> IIgredients.GetByRestrictions(IngredientsRequest ingredients)
        {
            List<IngredientsResponse> response = null;
            List<Areas> LstAreas = new List<Areas>();
            List<ProductToGet> LstProduct = new List<ProductToGet>();
            
            try
            {

                HttpContent content = new StringContent(JsonSerializer.Serialize(ingredients), System.Text.Encoding.UTF8, "application/json"); ;
                HttpResponseMessage responseJson = await clientHttp.PostAsync("https://geekburgeringredients20211216191440.azurewebsites.net/", content);

                responseJson.EnsureSuccessStatusCode();
                string responseBody = await responseJson.Content.ReadAsStringAsync();
                response = JsonSerializer.Deserialize<List<IngredientsResponse>>(responseBody);


                #region Area
                var AreasOn = await _production.GetAreas();
                 
                foreach (var itemArea in AreasOn.Where(x=>x.On))
                {
                    foreach (var itemIngredient in response)
                    {
                      if(!itemArea.Restrictions.Intersect(itemIngredient.Ingredients).Any())
                      {
                            LstAreas.Add(itemArea);
                      }
                    }                                                         
                }         
                    
                if(!LstAreas.Any())
                {
                    return null;
                }
                #endregion


                #region Products
                var produtos =  await _products.GetProducts("Morumbi");
                foreach (var itemproductId in response)
                {
                    var productsResponse = produtos.Where(x => x.ProductId == itemproductId.ProductId).FirstOrDefault();
                    if(productsResponse == null)
                    {
                        LstProduct.Add(productsResponse);
                    }                    
                }
                #endregion


                if(LstProduct.Count < 4)
                {
                    //TODO: Publish

                }


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
