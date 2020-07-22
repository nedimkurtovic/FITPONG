using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.SharedModels;
using FIT_PONG.WebAPI.Services.Bazni;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/sistemi-takmicenja")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class SistemiTakmicenjaController : BaseController<SharedModels.SistemiTakmicenja,object>
    {
        public SistemiTakmicenjaController(IBaseService<SharedModels.SistemiTakmicenja,object> _servis)
            :base(_servis)
        {

        }
    }
}
