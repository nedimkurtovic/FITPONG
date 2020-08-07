using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService reportsService;

        public ReportsController(IReportsService reportsService)
        {
            this.reportsService = reportsService;
        }


        [HttpGet]
        public PagedResponse<Reports> Get([FromQuery] ReportsSearch obj)
        {
            var rezult = GetPagedResponse(obj);
            return rezult;
        }

        [HttpGet("{id}")]
        public Reports Get(int id)
        {
            return reportsService.GetById(id);
        }

        [HttpPost]
        public Reports Add([FromBody]ReportsInsert obj)
        {
            return reportsService.Add(obj, "content"); 
        }
        private PagedResponse<Reports> GetPagedResponse(ReportsSearch obj)
        {
            var listaObjava = reportsService.Get(obj);
            PagedResponse<Reports> respons = new PagedResponse<Reports>();

            respons.TotalPageCount = (int)Math.Ceiling((double)listaObjava.Count() / (double)obj.Limit);
            respons.Stavke = listaObjava.Skip((obj.Page - 1) * obj.Limit).Take(obj.Limit).ToList();

            ReportsSearch iducaKlon = obj.Clone() as ReportsSearch;
            iducaKlon.Page = (iducaKlon.Page + 1) > respons.TotalPageCount ? -1 : iducaKlon.Page + 1;
            String iduciUrl = iducaKlon.Page == -1 ? null : this.Url.Action("Get", null, iducaKlon, Request.Scheme);

            ReportsSearch proslaKlon = obj.Clone() as ReportsSearch;
            proslaKlon.Page = (proslaKlon.Page - 1) < 0 ? -1 : proslaKlon.Page - 1;
            String prosliUrl = proslaKlon.Page == -1 ? null : this.Url.Action("Get", null, proslaKlon, Request.Scheme);

            respons.IducaStranica = !String.IsNullOrWhiteSpace(iduciUrl) ? new Uri(iduciUrl) : null;
            respons.ProslaStranica = !String.IsNullOrWhiteSpace(prosliUrl) ? new Uri(prosliUrl) : null;
            return respons;
        }
    }
}