using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.DataCache;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.Client.Services
{
    public class ProductService
    {
        IMemoryRepository _memoryRepository;

        public ProductService(IMemoryRepository repository)
        {
            _memoryRepository = repository;
        }

        public void Salvar(IEnumerable<ProductToGet> produtos)
        {
            foreach (var item in produtos)
            {
                _memoryRepository.Add<ProductToGet>(item.ProductId.ToString(), item);
            }
        }

        public ProductToGet Localizar(string id)
        {
            return _memoryRepository.GetById<ProductToGet>(id);
        }
    }
}
