using Org.BouncyCastle.Asn1.Mozilla;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Web.Http;

namespace FIT_PONG.Services.Bazni
{
    public interface ICRUDService<T,TInsert,TUpdate,TDb,TSearch> : IBaseService<T,TSearch>
    {
        public T Add(TInsert obj);
        public T Update(int id, TUpdate obj);
        public void Delete(int id);
    }
}
