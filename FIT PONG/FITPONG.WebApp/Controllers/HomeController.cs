using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FIT_PONG.Services;
using Microsoft.AspNetCore.Authorization;
using FIT_PONG.Services.BL;
using FIT_PONG.ViewModels.Home;
using Microsoft.EntityFrameworkCore;
using FIT_PONG.Database.DTOs;

namespace FIT_PONG.Controllers
{
    public class HomeController : Controller
    {
        /*ovdje ce se na pocetku prikazivati main dashboard,zadnje informacije ili whatever dakle trebat ce i ovdje 
         bazu omoguciti*/
        private readonly ILogger<HomeController> _logger;
        private readonly FIT_PONG.Database.MyDb db;
        private readonly FIT_PONG.Services.BL.Evidentor evidentor;
        public HomeController(ILogger<HomeController> logger , FIT_PONG.Database.MyDb instanca , FIT_PONG.Services.BL.Evidentor evidentorInstanca)
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
            model.TopIgraci = GetNajboljeOveSedmice();
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
                                    .Where(d => d.Utakmica.DatumVrijeme.Month == DateTime.Now.Month)
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
        //ovo za sad na apiju ne znam da li ima smisla, tj ima sigurno samo onda treba skontati gdje bi klasa TopIgraci
        //tj u koji bi se folder smjestila, da li to pripada takmicenju ili nekom ipak drugom servisu? Jer getnajboljiovesedmice
        //nema bas veze sa evidencijom
        private List<TopIgraciVM> GetNajboljeOveSedmice()
        {
            DateTime prosliPonedjeljak = GetProsliPonedjeljak(DateTime.Now);
            List<Utakmica> utakmiceUSedmici = db.Utakmice.AsNoTracking().Include(x => x.UcescaNaUtakmici)
                .Where(x => x.IsEvidentirana && x.DatumVrijeme >= prosliPonedjeljak && x.DatumVrijeme <= DateTime.Now)
                .ToList();
            List<TopIgraciVM> listaPovratnih = new List<TopIgraciVM>();
            foreach (Utakmica i in utakmiceUSedmici)
            {
                foreach (Igrac_Utakmica j in i.UcescaNaUtakmici)
                {
                    if (listaPovratnih.Where(x => x.IgracId == j.IgracID).Count() == 0)
                    {
                        Igrac_Utakmica ucesceNatekmi = db.IgraciUtakmice
                            .Include(x => x.TipRezultata).Include(x => x.Igrac)
                            .Where(x => x.IgID == j.IgID).FirstOrDefault();
                        listaPovratnih.Add(new TopIgraciVM
                        {
                            BrojOsvojenihSetova = 0,
                            BrojPobjeda = 0,
                            BrojPoraza = 0,
                            ELO = ucesceNatekmi.Igrac.ELO,
                            IgracId = ucesceNatekmi.IgracID.GetValueOrDefault(),
                            Naziv = ucesceNatekmi.Igrac.PrikaznoIme
                        });
                    }
                    Igrac_Utakmica ucesce = db.IgraciUtakmice.Include(x => x.TipRezultata)
                            .Where(x => x.IgID == j.IgID).FirstOrDefault();
                    TopIgraciVM obj = listaPovratnih.Where(x => x.IgracId == j.IgracID).FirstOrDefault();
                    bool pobjeda = (ucesce.TipRezultata.Naziv == "Pobjeda") ? true : false;
                    if (pobjeda)
                        obj.BrojPobjeda++;
                    else
                        obj.BrojPoraza++;
                    obj.BrojOsvojenihSetova += j.OsvojeniSetovi.GetValueOrDefault();
                }
            }
            return listaPovratnih.OrderByDescending(x => x.BrojOsvojenihSetova).ToList();
        }
        private DateTime GetProsliPonedjeljak(DateTime trenutno)
        {
            for (int i = 0; i < 7; i++)
            {
                if (trenutno.AddDays(-i).DayOfWeek == DayOfWeek.Monday)
                    return trenutno.AddDays(-i).Date;
            }
            return new DateTime(1000, 1, 1);
        }
    }
}
