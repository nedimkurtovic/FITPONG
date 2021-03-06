﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIT_PONG.Services;
using FIT_PONG.ViewModels.FeedVMs;
using FIT_PONG.ViewModels;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using FIT_PONG.Database.DTOs;
using FIT_PONG.SharedModels.Requests;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace FIT_PONG.Controllers
{
    public class FeedController : Controller
    {

        private readonly FIT_PONG.Database.MyDb db;
        public FeedController(FIT_PONG.Database.MyDb instanca)
        {
            db = instanca;
        }    
        public IActionResult Prikaz(int id,int page = 1)
        {
            Feed x = db.Feeds.Find(id);
            if(x == null)
                return Redirect("/Feed/Neuspjeh");

            DisplayFeedVM obj = new DisplayFeedVM
            {
                ID = x.ID,
                naziv = x.Naziv,
                DatumModifikacije = x.DatumModifikacije
            };
            var qry = (from o in db.Objave
                       join fo in db.FeedsObjave
                       on o.ID equals fo.ObjavaID
                       where fo.FeedID == x.ID
                       select fo.Objava).ToList().OrderByDescending(x => x.DatumKreiranja);
            obj.Objave = PagingList.Create(qry, 3, page);
            ViewBag.trenutna = page;
            ViewBag.ukupnoStranica = obj.Objave.PageCount;
            int kreator = db.Takmicenja.Where(x => x.FeedID == id).Select(x => x.KreatorID).FirstOrDefault();
            ViewBag.username = db.Users.Where(x => x.Id == kreator).Select(x => x.UserName).FirstOrDefault();
            //ViewBag.fid = obj;
            return PartialView(obj);

        }

        public IActionResult Neuspjeh()
        {
            ViewBag.poruka = "Feed ne postoji ili je nesto krenulo po zlu";
            return View();
        }
    }
}