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
            List<Grad> gradovi = db.Gradovi.ToList();
            ViewData["gradoviKey"] = gradovi;
            return View();
        }

        [HttpPost]
        public ActionResult Dodaj(Grad grad)
        {
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
        public ActionResult Dodaj()
        {
            return View();
        }

        public ActionResult Obrisi(int gradID)
        {
            Grad grad=db.Gradovi.Find(gradID);
            if (grad != null)
            {
                db.Remove(grad);
                db.SaveChanges();
            }
            
            return Redirect("/Grad");
        }

        [HttpGet]
        public ActionResult Edit(int gradID)
        {
            Grad grad = db.Gradovi.Find(gradID);
            if (grad == null)
            {
                return Redirect("/Grad");
            }
            return View(grad);
        }

        [HttpPost]
        public ActionResult Edit(int ID, Grad grad)
        {
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