using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.Controllers
{
    public class ErrorController : Controller
    {
        [AllowAnonymous]
        [Route("Error/{brojkoda}")]
        public IActionResult Index(int brojkoda)
        {
            switch(brojkoda)
            {
                case 403: ViewBag.poruka = "Zabranjena operacija"; break;
                case 404: ViewBag.poruka = "Trazeni resurs ne postoji"; break;
                case 500: ViewBag.poruka = "Doslo je do nevidjene greske"; break;
                case 507: ViewBag.poruka = "Memorija je pretrpana, obavijestite hitno administratora"; break;
                default: ViewBag.poruka = "Doslo je do greske nevidjenih razmjera";break;
            }
            return View("Neuspjeh");
        }
        
    }
}