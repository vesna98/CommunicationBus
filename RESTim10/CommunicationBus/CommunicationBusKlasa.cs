using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationBus
{
    public class CommunicationBusKlasa:ICommunicationBus
    {

        public string Zahtev { get; set; }
        public CommunicationBusKlasa(string zahtev)
        {
            Zahtev = zahtev;

        }
        public string Back(string poruka)
        {
            return poruka;
        }

        public string Forward()
        {
            return Zahtev;
        }
    }
}
