using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services.Services;
using FIT_PONG.Services.Services.Autorizacija;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
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
        public PagedResponse<Objave> Get([FromQuery]ObjaveSearch obj)
        {
            var rezult = GetPagedResponse(obj);
            return rezult;
        }

        [HttpGet("{id}")]
        public Objave GetById(int id)
        {
            return objaveService.GetById(id);
        }

        [HttpPost]
        public Objave Add(ObjaveInsertUpdate obj)
        {
            objaveAutorizator.AuthorizeAddGlavniFeed(usersService.GetRequestUserName(HttpContext.Request));
            return objaveService.Add(obj);
        }

        //[HttpPost]
        //public Objave Add(int FeedId, ObjaveInsertUpdate obj)
        //{
        //    return objaveService.Add(FeedId, obj);
        //}


        [HttpPut("{id}")]
        public Objave Edit(int id,[FromBody] ObjaveInsertUpdate obj)
        {
            return objaveService.Update(id, obj);
        }

        [HttpDelete("{id}")]
        public SharedModels.Objave Delete(int id)
        {
            return objaveService.Delete(id);
        }
        private PagedResponse<Objave> GetPagedResponse(ObjaveSearch obj)
        {
            var listaObjava = objaveService.Get(obj);
            PagedResponse<Objave> respons = new PagedResponse<Objave>();

            respons.TotalPageCount = (int)Math.Ceiling((double)listaObjava.Count() / (double)obj.Limit);
            respons.Stavke = listaObjava.Skip((obj.Page - 1) * obj.Limit).Take(obj.Limit).ToList();

            ObjaveSearch iducaKlon = obj.Clone() as ObjaveSearch;
            iducaKlon.Page = (iducaKlon.Page + 1) > respons.TotalPageCount ? -1 : iducaKlon.Page + 1;
            String iduciUrl = iducaKlon.Page == -1 ? null : this.Url.Action("Get", null, iducaKlon, Request.Scheme);

            ObjaveSearch proslaKlon = obj.Clone() as ObjaveSearch;
            proslaKlon.Page = (proslaKlon.Page - 1) < 0 ? -1 : proslaKlon.Page - 1;
            String prosliUrl = proslaKlon.Page == -1 ? null : this.Url.Action("Get", null, proslaKlon, Request.Scheme);

            respons.IducaStranica = !String.IsNullOrWhiteSpace(iduciUrl) ? new Uri(iduciUrl) : null;
            respons.ProslaStranica = !String.IsNullOrWhiteSpace(prosliUrl) ? new Uri(prosliUrl) : null;
            return respons;
        }
    }
}