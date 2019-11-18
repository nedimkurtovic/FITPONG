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
        public IActionResult Index()
        {
            MyDb db = new MyDb();

            List<Grad> gradovi = db.Gradovi.ToList();
            ViewData["gradoviKey"] = gradovi;
            db.Dispose();
            return View();
        }

        [HttpPost]
        public ActionResult Dodaj(Grad grad)
        {
            if (DaLiPostoji(grad.Naziv))
                return View("Greska");

            if (ModelState.IsValid)
            {
                MyDb db = new MyDb();
                db.Gradovi.Add(grad);
                db.SaveChanges();
                db.Dispose();
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
            MyDb db = new MyDb();
            Grad grad=db.Gradovi.Find(gradID);
            if (grad != null)
            {
                db.Remove(grad);
                db.SaveChanges();
            }
            db.Dispose();
            
            return Redirect("/Grad");
        }

        [HttpGet]
        public ActionResult Edit(int gradID)
        {
            MyDb db = new MyDb();
            Grad grad = db.Gradovi.Find(gradID);
            if (grad == null)
            {
                return Redirect("/Grad");
            }
            db.Dispose();
            return View(grad);
        }

        [HttpPost]
        public ActionResult Edit(int ID, Grad grad)
        {
            if (DaLiPostoji(grad.Naziv))
                return View("Greska");

            MyDb db = new MyDb();
            Grad g = db.Gradovi.Find(ID);
            if (g != null && ModelState.IsValid)
            {
                g.Naziv = grad.Naziv;
                db.SaveChanges();
                return Redirect("/Grad");
            }
            
            db.Dispose();

            return View(g);
        }

        bool DaLiPostoji(string naziv)
        {
            MyDb db = new MyDb();
            List<Grad> gradovi = db.Gradovi.ToList();
            foreach (var item in gradovi)
            {
                if (item.Naziv == naziv)
                {
                    db.Dispose();
                    return true;
                }
            }
            db.Dispose();
            return false;
        }

    }
}