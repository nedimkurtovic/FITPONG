using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.WebAPI.Services.Bazni;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/kategorije-takmicenja")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class KategorijeTakmicenjaController : BaseController<SharedModels.KategorijeTakmicenja, object>
    {
        public KategorijeTakmicenjaController(IBaseService<SharedModels.KategorijeTakmicenja,
            object> _servis): base(_servis)
        {

        }
    }
}
