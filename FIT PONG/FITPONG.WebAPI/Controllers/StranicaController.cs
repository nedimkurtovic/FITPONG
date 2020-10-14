using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.Services.Services.Autorizacija;
using FIT_PONG.SharedModels.Requests.Aktivnosti;
using FIT_PONG.SharedModels.Requests;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StranicaController : ControllerBase
    {
        private readonly IAktivnostiService aktivnostiService;
        private readonly IAktivnostiAutorizator aktivnostAutorizator;
        private readonly IUsersService usersService;

        public StranicaController(IAktivnostiService _AktivnostiService, IAktivnostiAutorizator _AktivnostAutorizator
            ,IUsersService _UsersService)
        {
            aktivnostiService = _AktivnostiService;
            aktivnostAutorizator = _AktivnostAutorizator;
            usersService = _UsersService;
        }
        [Route("aktivnosti")]
        [HttpGet]
        public PagedResponse<BrojKorisnikaLogs> GetAktivnosti([FromQuery]AktivnostiSearch obj)
        {
            var userEmail = usersService.GetRequestUserName(HttpContext.Request);
            aktivnostAutorizator.AuthorizeGet(userEmail);
            var respons = GetPagedResponse(obj);
            return respons;
        }
        [Route("stanje")]
        [HttpGet]
        public StanjeStranice GetStanje()
        {
            var userEmail = usersService.GetRequestUserName(HttpContext.Request);
            aktivnostAutorizator.AuthorizeGet(userEmail);
            StanjeStranice respons = aktivnostiService.GetStanjeStranice(10);
            return respons;
        }
        private PagedResponse<BrojKorisnikaLogs> GetPagedResponse(AktivnostiSearch obj)
        {
            var listaAktivnosti = aktivnostiService.Get(obj);
            PagedResponse<BrojKorisnikaLogs> respons = new PagedResponse<BrojKorisnikaLogs>();

            DateTime vrijeme = DateTime.Now.AddMonths(-(obj.Page - 1));
            respons.Stavke = listaAktivnosti
                .Where(x=>x.Datum.Year == vrijeme.Year 
                && x.Datum.Month == vrijeme.Month).OrderBy(x=>x.Datum).ToList();

            int brojac = 1;
            DateTime zadnjiDatum = DateTime.Now;
            //O(n), i hash mapa bi isto O(n), isto kao i unutrasnja for petlja za preskakanje
            //treba mi unique kombinacija year month tj count njihov
            for(int i =0;i<listaAktivnosti.Count;i++)
            {
                if(listaAktivnosti[i].Datum.Date.Year != zadnjiDatum.Date.Year
                    || listaAktivnosti[i].Datum.Month != zadnjiDatum.Date.Month)
                {
                    zadnjiDatum = listaAktivnosti[i].Datum.Date;
                    brojac++;
                }
            }
            respons.TotalPageCount = brojac;

            AktivnostiSearch iducaKlon = obj.Clone() as AktivnostiSearch;
            iducaKlon.Page = (iducaKlon.Page + 1) > respons.TotalPageCount ? -1 : iducaKlon.Page + 1;
            String iduciUrl = iducaKlon.Page == -1 ? null : this.Url.Action("GetAktivnosti","stranica", iducaKlon, Request.Scheme);

            AktivnostiSearch proslaKlon = obj.Clone() as AktivnostiSearch;
            proslaKlon.Page = (proslaKlon.Page - 1) < 0 ? -1 : proslaKlon.Page - 1;
            String prosliUrl = proslaKlon.Page == -1 ? null : this.Url.Action("GetAktivnosti", "stranica", proslaKlon, Request.Scheme);

            respons.IducaStranica = !String.IsNullOrWhiteSpace(iduciUrl) ? new Uri(iduciUrl) : null;
            respons.ProslaStranica = !String.IsNullOrWhiteSpace(prosliUrl) ? new Uri(prosliUrl) : null;
            return respons;
        }
    }
}
