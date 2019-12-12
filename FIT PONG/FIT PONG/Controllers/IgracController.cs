using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Models;
using FIT_PONG.ViewModels.IgracVMs;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.Controllers
{
    public class IgracController : Controller
    {
        private readonly MyDb db;

        public IgracController(MyDb context)
        {
            db = context;
        }

        public IActionResult Index(string searchBy, string search, string orderBy)
        {
            if (searchBy == "JacaRuka")
            {
                List<IgracVM> igraci = db.Igraci.Select(x => new IgracVM
                {
                    ID = x.ID,
                    BrojPosjetaNaProfil = x.BrojPosjetaNaProfil,
                    JacaRuka = x.JacaRuka,
                    PrikaznoIme = x.PrikaznoIme,
                    ProfileImagePath = x.ProfileImagePath,
                    Visina = x.Visina,
                    ELO = x.ELO
                }).Where(x => x.JacaRuka == search || search == null).ToList();
                igraci=Sort(igraci, orderBy);
                return View(igraci);
            }
            else
            {
                List<IgracVM> igraci = db.Igraci.Select(x => new IgracVM
                {
                    ID = x.ID,
                    BrojPosjetaNaProfil = x.BrojPosjetaNaProfil,
                    JacaRuka = x.JacaRuka,
                    PrikaznoIme = x.PrikaznoIme,
                    ProfileImagePath = x.ProfileImagePath,
                    Visina = x.Visina,
                    ELO = x.ELO
                }).Where(x => x.PrikaznoIme == search || search == null).OrderBy(x => x.PrikaznoIme).ToList();
                igraci=Sort(igraci, orderBy);
                return View(igraci);
            }
            
        }

        public IActionResult PrikazProfila(int id)
        {
            Igrac objIgrac = db.Igraci.SingleOrDefault(x => x.ID == id);
            if (objIgrac == null)
                return View("Igrac/Greska");

            IgracVM igrac = new IgracVM(objIgrac);
            return View(igrac);
        }
        
        private List<IgracVM> Sort(List<IgracVM> igraci, string sortBy)
        {
            switch (sortBy)
            {
                case "PrikaznoIme":
                    return igraci.OrderBy(x => x.PrikaznoIme).ToList();
                case "EloRaiting":
                    return igraci = igraci.OrderBy(x => x.ELO).ToList();
                case "BrojPosjetaNaProfil":
                    return igraci = igraci.OrderBy(x => x.BrojPosjetaNaProfil).ToList();
                default:
                    return igraci;
            }

        }
    }
}