using AutoMapper;
using FIT_PONG.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Services.Bazni
{
    public class BaseService<T,TDb, TSearch> : IBaseService<T, TSearch> where TDb:class
    {
        protected MyDb db;
        protected IMapper mapko;

        public BaseService(MyDb _db, IMapper _mapko)
        {
            db = _db;
            mapko = _mapko;
        }
        public virtual List<T> Get(TSearch obj = default)
        {
            return mapko.Map<List<T>>(db.Set<TDb>().ToList());
        }

        public virtual T GetById(int id)
        {
            var objBaza = db.Set<TDb>().Find(id);
            return mapko.Map<T>(objBaza);
        }
    }
}
