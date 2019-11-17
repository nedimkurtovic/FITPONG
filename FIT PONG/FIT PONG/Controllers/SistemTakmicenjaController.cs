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
        public IActionResult Index()
        {
            MyDb db = new MyDb();

            List<Sistem_Takmicenja> sistemi = db.SistemiTakmicenja.ToList();
            ViewData["sistemiKey"] = sistemi;
            db.Dispose();
            return View();
        }

        [HttpPost]
        public ActionResult Dodaj(Sistem_Takmicenja st)
        {
            if (ModelState.IsValid)
            {
                MyDb db = new MyDb();
                db.SistemiTakmicenja.Add(st);
                db.SaveChanges();
                db.Dispose();
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
            MyDb db = new MyDb();
            Sistem_Takmicenja st = db.SistemiTakmicenja.Find(id);
            if (st != null)
            {
                db.Remove(st);
                db.SaveChanges();
            }
            db.Dispose();

            return Redirect("/SistemTakmicenja");
        }

        [HttpGet]
        public ActionResult Edit(int id)
        {
            MyDb db = new MyDb();
            Sistem_Takmicenja st= db.SistemiTakmicenja.Find(id);
            if (st == null)
            {
                return Redirect("/SistemTakmicenja");
            }
            db.Dispose();
            return View(st);
        }

        [HttpPost]
        public ActionResult Edit(int id, Sistem_Takmicenja st)
        {
            MyDb db = new MyDb();
            Sistem_Takmicenja sistem_takmicenja = db.SistemiTakmicenja.Find(id);
            if (sistem_takmicenja != null && ModelState.IsValid)
            {
                sistem_takmicenja.Opis= st.Opis;
                db.SaveChanges();
                return Redirect("/SistemTakmicenja");
            }

            db.Dispose();

            return View(sistem_takmicenja);
        }

    }

}