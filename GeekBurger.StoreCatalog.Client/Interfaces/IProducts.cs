using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.Contract.Request;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Client.Interfaces
{
    public interface IProducts : IClientHttp
    {
        Task<ProductToGet> GetProducts(RequestProducts request);
    }
}
