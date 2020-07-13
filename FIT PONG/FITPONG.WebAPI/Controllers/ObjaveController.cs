using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services.Services;
using FIT_PONG.Services.Services.Autorizacija;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Objave;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class ObjaveController : ControllerBase
    {
        private readonly IObjaveService objaveService;
        private readonly IObjaveAutorizator objaveAutorizator;
        private readonly IUsersService usersService;


        public ObjaveController(IObjaveService objaveService, IUsersService usersService, IObjaveAutorizator objaveAutorizator)
        {
            this.objaveService = objaveService;
            this.usersService = usersService;
            this.objaveAutorizator = objaveAutorizator;
        }


        [HttpGet]
        public List<Objave> Get()
        {
            return objaveService.Get();
        }

        [HttpGet]
        [Route("{id}")]
        public Objave GetById(int id)
        {
            return objaveService.GetById(id);
        }

        [HttpPost]
        [Route("dodajNaGlavniFeed")]
        public Objave AddGlavniFeed(ObjaveInsertUpdate obj)
        {
            objaveAutorizator.AuthorizeAddGlavniFeed(usersService.GetEmail(HttpContext.Request));
            return objaveService.Add(obj);
        }

        [HttpPost]
        [Route("dodaj")]
        public Objave Add(int FeedId, ObjaveInsertUpdate obj)
        {
            return objaveService.Add(FeedId, obj);
        }


        [HttpPut]
        [Route("{id}/uredi")]
        public Objave Edit(int id, ObjaveInsertUpdate obj)
        {
            return objaveService.Update(id, obj);
        }

        [HttpDelete]
        [Route("obrisi")]
        public void Delete(int id)
        {
            objaveService.Delete(id);
        }
    }
}