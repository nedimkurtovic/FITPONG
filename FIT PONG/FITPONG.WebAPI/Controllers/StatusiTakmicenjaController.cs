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
    [Route("api/statusi-takmicenja")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class StatusiTakmicenjaController : BaseController<SharedModels.StatusiTakmicenja, object>
    {
        public StatusiTakmicenjaController(IBaseService<SharedModels.StatusiTakmicenja,
            object> _servis) : base(_servis)
        {

        }
    }
}
