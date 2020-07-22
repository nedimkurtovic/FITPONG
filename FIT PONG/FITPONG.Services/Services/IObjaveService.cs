using FIT_PONG.WebAPI.Services.Bazni;
using Microsoft.AspNetCore.Http.Features;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Services.Services
{
    public interface IObjaveService:IBaseService<SharedModels.Objave,object>
    {
        SharedModels.Objave Add(int FeedID, SharedModels.Requests.Objave.ObjaveInsertUpdate obj);
        SharedModels.Objave Add(SharedModels.Requests.Objave.ObjaveInsertUpdate obj);
        SharedModels.Objave Update(int id, SharedModels.Requests.Objave.ObjaveInsertUpdate obj);
        void Delete(int id);
        List<SharedModels.Objave> GetAll(int FeedID);
    }
}
