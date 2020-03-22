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
using Microsoft.EntityFrameworkCore;

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
            var objave = db.Objave.OrderByDescending(x => x.DatumKreiranja).Take(5).ToList();

            foreach (var item in objave)
            {
                FeedObjava f = db.FeedsObjave.Where(d => d.ObjavaID == item.ID).FirstOrDefault();
                if (f != null) {
                    Takmicenje t = db.Takmicenja.Where(d => d.FeedID == f.FeedID).FirstOrDefault();
                    model.ZadnjeObjave.Add((item, t));
                }
            }
            model.FunFacts = GetFunFacts();
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

        private List<String> GetFunFacts()
        {
            List<string> funfacts = new List<string>();
            List<Igrac_Utakmica> x = db.IgraciUtakmice
                                    .Include(d => d.Utakmica)
                                    .Include(d=>d.Igrac)
                                    .Where(d=>d.OsvojeniSetovi!=null)
                                    //.Where(d => d.Utakmica.DatumVrijeme.Month == DateTime.Now.Month)
                                    .OrderByDescending(d => d.OsvojeniSetovi).ToList();
            if (x.Count != 0)
            {
                int max = x.First().OsvojeniSetovi.GetValueOrDefault();
                int min = x.Last().OsvojeniSetovi.GetValueOrDefault();
                foreach (var item in x)
                {
                    if (item.OsvojeniSetovi != max)
                        break;
                    funfacts.Add("Igrač " + item.Igrac.PrikaznoIme + " osvojio je " + item.OsvojeniSetovi + " seta/ova.");
                }
                x = x.OrderBy(d => d.OsvojeniSetovi).ToList();
                foreach (var item in x)
                {
                    if (item.OsvojeniSetovi != min)
                        break;
                    funfacts.Add("Igrač " + item.Igrac.PrikaznoIme + " osvojio je " + item.OsvojeniSetovi + " set/a/ova. ");
                }
            }
            return funfacts;
        }
    }
}
