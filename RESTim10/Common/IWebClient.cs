using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
  public  interface IWebClient
    {
        string Zahtev { get; set; }
        string ConvertToJSON();
        string ToDoFilter(string poruka);
        string FieldsFirst(string poruka);
        void Show(string poruka);
    }
}
