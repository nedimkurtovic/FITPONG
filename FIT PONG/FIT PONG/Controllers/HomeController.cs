using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FIT_PONG.Models;
using Microsoft.AspNetCore.Authorization;
using FIT_PONG.Models.BL;
using FIT_PONG.ViewModels.Home;

namespace FIT_PONG.Controllers
{
    public class HomeController : Controller
    {
        /*ovdje ce se na pocetku prikazivati main dashboard,zadnje informacije ili whatever dakle trebat ce i ovdje 
         bazu omoguciti*/
        private readonly ILogger<HomeController> _logger;
        private readonly MyDb db;
        private readonly Evidentor evidentor;
        public HomeController(ILogger<HomeController> logger , MyDb instanca , Evidentor evidentorInstanca)
        {
            _logger = logger;
            db = instanca;
            evidentor = evidentorInstanca;
        }
        [AllowAnonymous]
        public IActionResult Index()
        {
            HomeIndexVM model = new HomeIndexVM();
            model.ZadnjeObjave = db.Objave.OrderByDescending(x => x.DatumKreiranja).Take(5).ToList();
            model.ZadnjiRezultati = evidentor.GetZadnjeUtakmice(10);
            return View(model);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
