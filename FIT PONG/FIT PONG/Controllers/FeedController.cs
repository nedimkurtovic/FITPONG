using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIT_PONG.Models;
using FIT_PONG.ViewModels.FeedVMs;
using FIT_PONG.ViewModels;
using Microsoft.EntityFrameworkCore;

namespace FIT_PONG.Controllers
{
    public class FeedController : Controller
    {
        public IActionResult Index()
        {
            MyDb db = new MyDb();
            List < DisplayFeedVM > feeds = db.Feeds.Select(s => new DisplayFeedVM
            {
                ID = s.ID,
                naziv = s.Naziv,
                DatumModifikacije = s.DatumModifikacije
            }).ToList();
            ViewBag.feeds = feeds;
            return View();
        }
        [HttpPost]
        public IActionResult Dodaj(CreateFeedVM novi)
        {
            if(PostojiIsti(novi.Naziv))
            {
                return View();
            }
            if(ModelState.IsValid)
            {
                Feed obj = new Feed
                {
                    Naziv = novi.Naziv,
                    DatumModifikacije = DateTime.Now
                };
                MyDb db = new MyDb();
                db.Feeds.Add(obj);
                db.SaveChanges();
                db.Dispose();
                return Redirect("/Feed/Index");
            }
            return View(novi);
        }
        public IActionResult Dodaj()
        {
            MyDb db = new MyDb();
            ViewBag.takmicenja = db.Takmicenja.Select(s=> new ComboBoxVM
            {
                ID= s.ID,
                Opis = s.Naziv
            }).ToList();
            return View();
        }
        public IActionResult Prikaz(int id)
        {
            MyDb db = new MyDb();
            Feed x = db.Feeds.Find(id);
            if(x != null)
            {
                DisplayFeedVM obj = new DisplayFeedVM
                {
                    ID = x.ID,
                    naziv = x.Naziv,
                    DatumModifikacije = x.DatumModifikacije
                };
                obj.Objave = (from o in db.Objave
                              join fo in db.FeedsObjave
                              on o.ID equals fo.ObjavaID
                              where fo.FeedID == x.ID
                              select fo.Objava).ToList();
                ViewBag.fid = obj;
                return View();
            }
            return Redirect("/Feed/Neuspjeh");
        }
        [HttpPost]
        public IActionResult Edit(Feed novi)
        {
            if(PostojiIsti(novi.Naziv))
            {
                return View("Neuspjeh");
            }

            if(ModelState.IsValid)
            {
                MyDb db = new MyDb();
                Feed obj = db.Feeds.Find(novi.ID);
                obj.Naziv = novi.Naziv;
                obj.DatumModifikacije = DateTime.Now;
                db.Update(obj);
                db.SaveChanges();
                db.Dispose();
                //ovdje ce naravno preusmjerit na detalje TODO,prikazati objave itd..
                return Redirect("/Feed/Prikaz/"+obj.ID);
            }
            return View("Neuspjeh");
        }
        public IActionResult Edit(int id)
        {
            MyDb db = new MyDb();
            Feed obj = db.Feeds.Find(id);
            if(obj != null)
            {
                return View(obj);
            }
            return View("/Feed");
        }
        private bool PostojiIsti(string naziv)
        {
            MyDb db = new MyDb();
            if(db.Feeds.Where(s=> s.Naziv == naziv).Count() > 1)
            {
                db.Dispose();
                return true;
            }
            db.Dispose();
            return false;
        }
        public IActionResult Obrisi(int id)
        {
            MyDb db = new MyDb();
            Feed obj = db.Feeds.Find(id);
            if(obj != null)
            {
                DisplayFeedVM prikaz = new DisplayFeedVM { ID = obj.ID, naziv = obj.Naziv };
                ViewBag.feed = prikaz;
                return View();
            }
            return Redirect("/Feed/Neuspjeh");
        }
        public IActionResult PotvrdaBrisanja(int id)
        {
            MyDb db = new MyDb();
            Feed obj = db.Feeds.Find(id);
            if(obj != null)
            {
                db.Remove(obj);
                db.SaveChanges();
                db.Dispose();
                return Redirect("Index");
            }
            return Redirect("Neuspjeh");
        }
        public IActionResult Uspjeh()
        {
            return View();
        }
        public IActionResult Neuspjeh()
        {
            return View();
        }
    }
}