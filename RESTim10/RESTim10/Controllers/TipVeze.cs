using System;
using System.Collections.Generic;

namespace RESTim10.Controllers
{
    public partial class TipVeze
    {
        public int TipVezeId { get; set; }
        public string NazivVeze { get; set; }

        public override string ToString()
        {
            return "VEZA:" + NazivVeze;
        }
    }
}
