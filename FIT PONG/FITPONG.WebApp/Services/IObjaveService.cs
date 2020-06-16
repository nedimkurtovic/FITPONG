using FIT_PONG.Services.Bazni;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Services
{
    public interface IObjaveService:IBaseService<SharedModels.Objave,object>
    {
        public SharedModels.Objave Add(int FeedID, SharedModels.Requests.Objave.ObjaveInsertUpdate obj);
        public SharedModels.Objave Add(SharedModels.Requests.Objave.ObjaveInsertUpdate obj);
        public SharedModels.Objave Update(int id, SharedModels.Requests.Objave.ObjaveInsertUpdate obj);
        public void Delete(int id);
    }
}
