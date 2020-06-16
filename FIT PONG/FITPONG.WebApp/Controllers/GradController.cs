using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Models;
using Microsoft.AspNetCore.Mvc;
using FIT_PONG.Database.DTOs;
using FIT_PONG.Services;
using FIT_PONG.SharedModels.Requests.Gradovi;
using FIT_PONG.Filters;
using FITPONG.Services;

namespace FIT_PONG.Controllers
{

    public class GradController : Controller
    {
        private readonly IGradoviService gradoviServis;

        public GradController(IGradoviService _GradoviServis)
        {
            gradoviServis = _GradoviServis;
        }

        public IActionResult Index()
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();
            var gradovi = gradoviServis.Get();
            ViewData["gradoviKey"] = gradovi;
            return View();
        }

        [HttpPost]
        public IActionResult Dodaj(GradoviInsertUpdate grad)
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();
            if (!ModelState.IsValid)
                return View(grad);
            try
            {
                if(gradoviServis.Add(grad) != null)
                    return Redirect("/Grad");
            }
            catch (UserException exc)
            {
                foreach ((string key, string msg) x in exc.Errori)
                {
                    ModelState.AddModelError(x.key, x.msg);
                }
            }
            return View(grad);
        }

        [HttpGet]
        public IActionResult Dodaj()
        {
            if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
                return VratiNijeAutorizovan();
            return View();
        }

        //public IActionResult Neuspjeh(string poruka)
        //{
        //    ViewBag.poruka = poruka;
        //    return View();
        //}


        public IActionResult VratiNijeAutorizovan()
        {
            ViewBag.poruka = "Niste autorizovani za takvu radnju";
            return View("Neuspjeh");
        }

        //public IActionResult Obrisi(int gradID)
        //{
        //    if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
        //        return VratiNijeAutorizovan();
        //    Grad grad=db.Gradovi.Find(gradID);
        //    if (grad != null)
        //    {
        //        db.Remove(grad);
        //        db.SaveChanges();
        //    }

        //    return Redirect("/Grad");
        //}

        //[HttpGet]
        //public IActionResult Edit(int gradID)
        //{
        //    if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
        //        return VratiNijeAutorizovan();
        //    Grad grad = db.Gradovi.Find(gradID);
        //    if (grad == null)
        //    {
        //        return Redirect("/Grad");
        //    }
        //    return View(grad);
        //}

        //[HttpPost]
        //public IActionResult Edit(int ID, Grad grad)
        //{
        //    if (User.Identity.Name != "aldin.talic@edu.fit.ba" && User.Identity.Name != "nedim.kurtovic@edu.fit.ba")
        //        return VratiNijeAutorizovan();
        //    if (DaLiPostoji(grad.Naziv))
        //        return View("Greska");

        //    Grad g = db.Gradovi.Find(ID);
        //    if (g != null && ModelState.IsValid)
        //    {
        //        g.Naziv = grad.Naziv;
        //        db.SaveChanges();
        //        return Redirect("/Grad");
        //    }

        //    return View(g);
        //}

        //bool DaLiPostoji(string naziv)
        //{
        //    List<Grad> gradovi = db.Gradovi.ToList();
        //    foreach (var item in gradovi)
        //    {
        //        if (item.Naziv == naziv)
        //            return true;
        //    }

        //    return false;
        //}

    }
}