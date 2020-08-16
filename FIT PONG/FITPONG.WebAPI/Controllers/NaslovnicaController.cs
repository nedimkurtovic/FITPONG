using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using FIT_PONG.Services.BL;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.WebAPI.Services.Bazni;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class NaslovnicaController : ControllerBase
    {
        private readonly Evidentor evidentor;
        private readonly IObjaveService objaveServis;

        public NaslovnicaController(Evidentor _evidentor, IObjaveService _objaveServis)
        {
            evidentor = _evidentor;
            objaveServis = _objaveServis;
        }

        [HttpGet]
        public SharedModels.Naslovnica Get()
        {
            Naslovnica povratna = new Naslovnica();
            povratna.ZadnjiRezultati = evidentor.GetZadnjeUtakmice(10);
            povratna.ZadnjeObjave = objaveServis.Get().Take(10).ToList();
            return povratna;
        }
    }
}
