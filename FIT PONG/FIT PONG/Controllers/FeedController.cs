using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using FIT_PONG.Models;


namespace FIT_PONG.Controllers
{
    public class FeedController : Controller
    {
        public IActionResult Index()
        {
            MyDb db = new MyDb();
            
            return View();
        }
        public IActionResult Dodaj(CreateFeedVM novi)
        {
            
        }
        public IActionResult Dodaj()
        {
            return View();
        }
        public IActionResult Edit(int id)
        {
            MyDb db = new MyDb();
            Feed obj = db.Feeds.Find(id);
            if(obj != null)
            {
                return View(obj);
            }
        }
    }
}