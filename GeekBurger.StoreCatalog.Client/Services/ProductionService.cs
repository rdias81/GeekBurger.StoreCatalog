using GeekBurger.Production.Contract;
using GeekBurger.StoreCatalog.DataCache;
using System.Collections.Generic;

namespace GeekBurger.StoreCatalog.Client.Services
{
    public class ProductionService
    {
        IMemoryRepository _memoryRepository;

        public ProductionService(IMemoryRepository repository)
        {
            _memoryRepository = repository;
        }

        public void Salvar(IEnumerable<Areas> areas)
        {
            foreach (Areas area in areas)
            {
                _memoryRepository.Add<Areas>(area.ProductionId.ToString(), area);
            }
        }
        public Areas Localizar(string id)
        {
            return _memoryRepository.GetById<Areas>(id);
        }
    }
}
