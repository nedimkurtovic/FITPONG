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
    [Route("api/vrste-takmicenja")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class VrsteTakmicenjaController : BaseController<SharedModels.VrsteTakmicenja, object>
    {
        public VrsteTakmicenjaController(IBaseService<SharedModels.VrsteTakmicenja, object> _servis)
            : base(_servis)
        {

        }
    }
}
