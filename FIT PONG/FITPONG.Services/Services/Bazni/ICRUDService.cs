using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.WebAPI.Services.Bazni
{
    public interface ICRUDService<T,TInsert,TUpdate,TSearch> : IBaseService<T,TSearch>
    {
        T Add(TInsert obj);
        T Update(int id, TUpdate obj);
        void Delete(int id);
    }
}
