using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services.Services;
using FIT_PONG.Services.Services.Autorizacija;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Gradovi;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class GradoviController : ControllerBase
    {
        private readonly IGradoviService gradoviService;
        private readonly IUsersService usersService;
        private readonly IGradoviAutorizator gradoviAutorizator;


        public GradoviController(IGradoviService gradoviService, IUsersService usersService, IGradoviAutorizator gradoviAutorizator)
        {
            this.gradoviService = gradoviService;
            this.usersService = usersService;
            this.gradoviAutorizator = gradoviAutorizator;
        }

        [HttpGet]
        public List<Gradovi> Get()
        {
            //gradoviAutorizator.AuthorizeGet(usersService.GetRequestUserName(HttpContext.Request));
            return gradoviService.Get();
        }

        [HttpGet("{id}")]
        public Gradovi Get(int id)
        {
            //gradoviAutorizator.AuthorizeGetById(usersService.GetRequestUserName(HttpContext.Request));
            return gradoviService.GetById(id);
        }


        [HttpPost]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public Gradovi Add(GradoviInsertUpdate obj)
        {
            gradoviAutorizator.AuthorizeAdd(usersService.GetRequestUserName(HttpContext.Request));
            return gradoviService.Add(obj);
        }

        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public Gradovi Update(int id, GradoviInsertUpdate obj)
        {
            gradoviAutorizator.AuthorizeUpdate(usersService.GetRequestUserName(HttpContext.Request));
            return gradoviService.Update(id, obj);
        }

        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        public void Delete(int id)
        {
            gradoviAutorizator.AuthorizeDelete(usersService.GetRequestUserName(HttpContext.Request));
            gradoviService.Delete(id);
        }
    }
}