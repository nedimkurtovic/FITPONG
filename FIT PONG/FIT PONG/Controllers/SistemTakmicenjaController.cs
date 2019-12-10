using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Models;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.Controllers
{
    public class SistemTakmicenjaController : Controller
    {
        private readonly MyDb db;

        public SistemTakmicenjaController(MyDb context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            List<Sistem_Takmicenja> sistemi = db.SistemiTakmicenja.ToList();
            ViewData["sistemiKey"] = sistemi;
            return View();
        }

        [HttpPost]
        public ActionResult Dodaj(Sistem_Takmicenja st)
        {
            if (DaLiPostoji(st.Opis))
                return View("Greska");

            if (ModelState.IsValid)
            {
                db.SistemiTakmicenja.Add(st);
                db.SaveChanges();
                return Redirect("/SistemTakmicenja");
            }
            return View();
        }

        [HttpGet]
        public ActionResult Dodaj()
        {
            return View();
        }

        public ActionResult Obrisi(int id)
        {
            Sistem_Takmicenja st = db.SistemiTakmicenja.Find(id);
            if (st != null)
            {
                db.Remove(st);
                db.SaveChanges();
            }

            return Redirect("/SistemTakmicenja");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            Sistem_Takmicenja st = db.SistemiTakmicenja.Find(id);
            if (st == null)
                return Redirect("/SistemTakmicenja");

            return View(st);
        }

        [HttpPost]
        public ActionResult Edit(int id, Sistem_Takmicenja st)
        {
            if (DaLiPostoji(st.Opis))
                return View("Greska");

            Sistem_Takmicenja sistem_takmicenja = db.SistemiTakmicenja.Find(id);
            if (sistem_takmicenja != null && ModelState.IsValid)
            {
                sistem_takmicenja.Opis = st.Opis;
                db.SaveChanges();
                return Redirect("/SistemTakmicenja");
            }

            return View(sistem_takmicenja);
        }

        bool DaLiPostoji(string opis)
        {
            List<Sistem_Takmicenja> sistemi = db.SistemiTakmicenja.ToList();
            foreach (var item in sistemi)
            {
                if (item.Opis == opis)
                    return true;
            }
            return false;
        }

    }

}