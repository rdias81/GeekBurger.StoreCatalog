using GeekBurger.Products.Contract;
using GeekBurger.StoreCatalog.DataCache;
using System.Collections.Generic;

namespace GeekBurger.StoreCatalog.Services
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

        public ProductToGet TesteGet(string id)
        {
            return _memoryRepository.GetById<ProductToGet>(id);
        }
    }
}
