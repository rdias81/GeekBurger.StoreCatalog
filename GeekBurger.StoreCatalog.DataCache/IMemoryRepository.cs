using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GeekBurger.StoreCatalog.DataCache
{
    public interface IMemoryRepository
    {
        object GetById(int id);
        void RemoveById(int id);
        object Add(object objeto);
       
    }
}
