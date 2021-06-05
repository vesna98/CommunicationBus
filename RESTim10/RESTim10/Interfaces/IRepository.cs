using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTim10.Interfaces
{
   public interface IRepository<T>
    {
        IEnumerable<T> GetAll();
        void Add(T toAdd);
        void Update(T toUpdate);
        void Delete(T toDelete);
        int Save();
    }
}
