using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Services.Bazni
{
    public interface IBaseService<T,TSearch>
    {
        public List<T> Get(TSearch obj = default(TSearch));
        public T GetById(int id);
    }
}
