using FIT_PONG.WebAPI.Services.Bazni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.SharedModels;

namespace FIT_PONG.Services.Services
{
    public interface IGradoviService : 
        ICRUDService<SharedModels.Gradovi,SharedModels.Requests.Gradovi.GradoviInsertUpdate,
            SharedModels.Requests.Gradovi.GradoviInsertUpdate,object>
    {
    }
}
