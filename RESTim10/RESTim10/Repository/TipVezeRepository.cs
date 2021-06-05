using PROTOTIP_NOVI_RES.Repository;
using RESTim10.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RESTim10.Repository
{
    public class TipVezeRepository : RepositoryKlasa<TipVeze, RESContext>
    {
        public TipVezeRepository(RESContext ctx) : base(ctx)
        {

        }
    }
}
