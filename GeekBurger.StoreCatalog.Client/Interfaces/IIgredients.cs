using GeekBurger.Ingredients.Contract.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Client.Interfaces
{
    public interface IIgredients : IClientHttp
    {
        Task<List<IngredientsResponse>> GetByRestrictions(IngredientsRequest ingredients);
    }
}
