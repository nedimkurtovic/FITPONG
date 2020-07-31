using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.WebAPI.Services.Bazni;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BaseController<T,TSearch> : ControllerBase
    {
        private readonly IBaseService<T, TSearch> servis;

        public BaseController(IBaseService<T,TSearch> _servis)
        {
            servis = _servis;
        }
        [HttpGet]
        public virtual List<T> Get([FromQuery] TSearch SearchObj)
        {
            return servis.Get(SearchObj);
        }
        [HttpGet("{id}")]
        public virtual T GetById(int id)
        {
            return servis.GetById(id);
        }
    }
}