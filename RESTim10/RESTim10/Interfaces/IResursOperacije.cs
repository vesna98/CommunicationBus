using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTim10.Interfaces
{
   public interface IResursOperacije
    {
        string Odgovor { get; set; }
        void GetOne(string zahtev);
        void Insert(string zahtev);
        void Update(string zahtev);
        void Delete(string zahtev);
    }
}
