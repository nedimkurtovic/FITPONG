using FIT_PONG.Services.Bazni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.SharedModels;

namespace FIT_PONG.Services
{
    public interface IGradoviService : 
        ICRUDService<SharedModels.Gradovi,SharedModels.Requests.Gradovi.GradoviInsertUpdate,
            SharedModels.Requests.Gradovi.GradoviInsertUpdate,Database.DTOs.Grad,object>
    {
    }
}
