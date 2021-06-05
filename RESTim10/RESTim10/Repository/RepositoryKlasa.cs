using Microsoft.EntityFrameworkCore;
using RESTim10.Controllers;
using RESTim10.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PROTOTIP_NOVI_RES.Repository
{
    public abstract class RepositoryKlasa<T, TC> : IRepository<T> where T : class where TC : RESContext
    {

        private readonly TC _ctx;
        public RepositoryKlasa(TC ctx)
        {
            _ctx = ctx;
        }

        public void Add(T toAdd)
        {
            _ctx.Set<T>().Add(toAdd);
        }

        public void Delete(T toDelete)
        {
            _ctx.Set<T>().Remove(toDelete);
        }

        public IEnumerable<T> GetAll()
        {
            IEnumerable<T> query = _ctx.Set<T>();
            return query;
        }

        public int Save()
        {

            return _ctx.SaveChanges();
        }

        public void Update(T toUpdate)
        {
            _ctx.Entry(toUpdate).State = EntityState.Modified;
        }
    }
}
