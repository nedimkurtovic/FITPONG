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
    [Route("api/vrste-suspenzija")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class VrsteSuspenzijaController : BaseController<SharedModels.VrsteSuspenzija, object>
    {
        public VrsteSuspenzijaController(IBaseService<SharedModels.VrsteSuspenzija,
            object> _servis): base(_servis)
        {

        }
    }
}
