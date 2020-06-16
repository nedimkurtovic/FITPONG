using AutoMapper;
using FIT_PONG.Models;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Services.Bazni
{
    public class CRUDService<T,TInsert,TUpdate,TDb,TSearch> : 
        BaseService<T,TDb,TSearch>,
        ICRUDService<T,TInsert,TUpdate,TDb,TSearch>
        where TDb : class
    {

        public CRUDService(MyDb _db, IMapper _mapko):base(_db,_mapko)
        {
            db = _db;
            mapko = _mapko;
        }

        public virtual T Add(TInsert obj)
        {
            var bazaObj = mapko.Map<TDb>(obj);
            db.Set<TDb>().Add(bazaObj);
            db.SaveChanges();
            return mapko.Map<T>(bazaObj);
        }

        public virtual void Delete(int id)
        {
            var bazaObj = db.Set<TDb>().Find(id);
            if(bazaObj != null)
                db.Set<TDb>().Remove(bazaObj);
            db.SaveChanges();
        }

        public virtual T Update(int id, TUpdate obj)
        {
            var bazaObj = db.Set<TDb>().Find(id);
            mapko.Map(obj, bazaObj);
            db.SaveChanges();
            return mapko.Map<T>(bazaObj);
        }
    }
}
