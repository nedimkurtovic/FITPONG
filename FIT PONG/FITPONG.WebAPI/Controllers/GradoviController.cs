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
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
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
            gradoviAutorizator.AuthorizeGet(usersService.GetEmail(HttpContext.Request));
            return gradoviService.Get();
        } 

        [HttpPost]
        public Gradovi Add(GradoviInsertUpdate obj)
        {
            gradoviAutorizator.AuthorizeAdd(usersService.GetEmail(HttpContext.Request));
            return gradoviService.Add(obj);
        }

        [HttpPut]
        public Gradovi Update(int id, GradoviInsertUpdate obj)
        {
            gradoviAutorizator.AuthorizeUpdate(usersService.GetEmail(HttpContext.Request));
            return gradoviService.Update(id, obj);
        }

        [HttpDelete]
        public void Delete(int id)
        {
            gradoviAutorizator.AuthorizeDelete(usersService.GetEmail(HttpContext.Request));
            gradoviService.Delete(id);
        }
    }
}