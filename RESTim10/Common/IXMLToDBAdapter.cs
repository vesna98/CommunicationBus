using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
   public interface IXMLToDBAdapter
    {
        string ConvertToDB();
        string BackToXML(string poruka);
        string Zahtev { get; set; }
    }
}
