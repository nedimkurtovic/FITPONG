using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.WebAPI.Services.Bazni
{
    public interface IBaseService<T,TSearch>
    {
        List<T> Get(TSearch obj = default(TSearch));
        T GetById(int id);
    }
}
