using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Models;
using Microsoft.AspNetCore.Mvc;
using FIT_PONG.Database.DTOs;

namespace FIT_PONG.Controllers
{
    public class SistemTakmicenjaController : Controller
    {
        private readonly FIT_PONG.Database.MyDb db;

        public SistemTakmicenjaController(FIT_PONG.Database.MyDb context)
        {
            db = context;
        }

        public IActionResult Index()
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();
            List<Sistem_Takmicenja> sistemi = db.SistemiTakmicenja.ToList();
            ViewData["sistemiKey"] = sistemi;
            return View();
        }

        [HttpPost]
        public IActionResult Dodaj(Sistem_Takmicenja st)
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();

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
        public IActionResult Dodaj()
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();

            return View();
        }

        public IActionResult Obrisi(int id)
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();

            Sistem_Takmicenja st = db.SistemiTakmicenja.Find(id);
            if (st != null)
            {
                db.Remove(st);
                db.SaveChanges();
            }

            return Redirect("/SistemTakmicenja");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();

            Sistem_Takmicenja st = db.SistemiTakmicenja.Find(id);
            if (st == null)
                return Redirect("/SistemTakmicenja");

            return View(st);
        }

        [HttpPost]
        public IActionResult Edit(int id, Sistem_Takmicenja st)
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();

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

    }

}