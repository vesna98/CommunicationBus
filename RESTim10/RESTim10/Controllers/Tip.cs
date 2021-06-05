using System;
using System.Collections.Generic;

namespace RESTim10.Controllers
{
    public partial class Tip
    {
        public int IdTip { get; set; }
        public string NazivTip { get; set; }

        public override string ToString()
        {
            return "TIP: " + NazivTip;
        }
    }
}
