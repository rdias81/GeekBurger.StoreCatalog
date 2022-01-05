using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.DataCache
{
    public interface IMemoryRepository
    {
        T GetById<T>(string id);
        
        void Add<T>(string id, T item);

    }
}
