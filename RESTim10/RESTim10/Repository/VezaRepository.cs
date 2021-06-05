using PROTOTIP_NOVI_RES.Repository;
using RESTim10.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTim10.Repository
{
    public class VezaRepository : RepositoryKlasa<Veza, RESContext>
    {
        public VezaRepository(RESContext ctx) : base(ctx)
        {

        }
    }

}
