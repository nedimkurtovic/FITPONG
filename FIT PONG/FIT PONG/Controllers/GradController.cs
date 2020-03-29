using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Models;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.Controllers
{

    public class GradController : Controller
    {
        private readonly MyDb db;

        public GradController(MyDb context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();
            List<Grad> gradovi = db.Gradovi.ToList();
            ViewData["gradoviKey"] = gradovi;
            return View();
        }

        [HttpPost]
        public IActionResult Dodaj(Grad grad)
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();
            if (DaLiPostoji(grad.Naziv))
                return View("Greska");

            if (ModelState.IsValid)
            {
                db.Gradovi.Add(grad);
                db.SaveChanges();
                return Redirect("/Grad");
            }
            return View();
        }

        [HttpGet]
        public IActionResult Dodaj()
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();
            return View();
        }

        public IActionResult Neuspjeh(string poruka)
        {
            ViewBag.poruka = poruka;
            return View();
        }


        public IActionResult VratiNijeAutorizovan()
        {
            ViewBag.poruka = "Niste autorizovani za takvu radnju";
            return View("Neuspjeh");
        }

        public IActionResult Obrisi(int gradID)
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();
            Grad grad=db.Gradovi.Find(gradID);
            if (grad != null)
            {
                db.Remove(grad);
                db.SaveChanges();
            }
            
            return Redirect("/Grad");
        }

        [HttpGet]
        public IActionResult Edit(int gradID)
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();
            Grad grad = db.Gradovi.Find(gradID);
            if (grad == null)
            {
                return Redirect("/Grad");
            }
            return View(grad);
        }

        [HttpPost]
        public IActionResult Edit(int ID, Grad grad)
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();
            if (DaLiPostoji(grad.Naziv))
                return View("Greska");

            Grad g = db.Gradovi.Find(ID);
            if (g != null && ModelState.IsValid)
            {
                g.Naziv = grad.Naziv;
                db.SaveChanges();
                return Redirect("/Grad");
            }
            
            return View(g);
        }

        bool DaLiPostoji(string naziv)
        {
            List<Grad> gradovi = db.Gradovi.ToList();
            foreach (var item in gradovi)
            {
                if (item.Naziv == naziv)
                    return true;
            }
            
            return false;
        }

    }
}