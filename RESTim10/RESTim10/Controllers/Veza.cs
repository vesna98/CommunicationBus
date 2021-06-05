using System;
using System.Collections.Generic;

namespace RESTim10.Controllers
{
    public partial class Veza
    {
        public int IdVeze { get; set; }
        public int IdPrvog { get; set; }
        public int IdDrugog { get; set; }
        public int TipVezeId { get; set; }

        public override string ToString()
        {

            return "TIP_VEZE_: " + TipVezeId;
        }
    }
}
