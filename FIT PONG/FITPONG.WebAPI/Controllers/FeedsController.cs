using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Objave;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class FeedsController : ControllerBase
    {
        private readonly IFeedsService feedsService;
        private readonly IObjaveService objaveService;


        public FeedsController(IFeedsService feedsService, IObjaveService objaveService)
        {
            this.feedsService = feedsService;
            this.objaveService = objaveService;
        }

        [HttpGet]
        public List<Feeds> Get()
        {
            return feedsService.Get();
        }

        [HttpGet("{id}")]
        public Feeds Get(int id)
        {
            return feedsService.GetById(id);
        }


        [HttpPost("{id}/objave")]
        public Objave Add(int id, ObjaveInsertUpdate obj)
        {
            return objaveService.Add(id, obj);
        }
        [HttpGet("{id}/objave")]
        public PagedResponse<Objave> GetObjave(int id,[FromQuery]ObjaveSearch obj)
        {
            var rezultat = GetPagedResponse(id, obj);
            return rezultat;
        }
        private PagedResponse<SharedModels.Objave> GetPagedResponse(int feedid,ObjaveSearch obj)
        {
            var listaTakmicenja = objaveService.GetAll(feedid,obj);
            PagedResponse<SharedModels.Objave> respons = new PagedResponse<SharedModels.Objave>();

            respons.TotalPageCount = (int)Math.Ceiling((double)listaTakmicenja.Count() / (double)obj.Limit);
            respons.Stavke = listaTakmicenja.Skip((obj.Page - 1) * obj.Limit).Take(obj.Limit).ToList();

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