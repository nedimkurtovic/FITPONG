using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services;
using FIT_PONG.ViewModels.ReportVMs;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.IO;
using Microsoft.EntityFrameworkCore;
using MailKit.Net.Smtp;
using MimeKit;
using FIT_PONG.Services.BL;
using Microsoft.AspNetCore.Authorization;
using FIT_PONG.Database.DTOs;

namespace FIT_PONG.Controllers
{
    [AllowAnonymous]
    public class ReportController : Controller
    {
        private readonly FIT_PONG.Database.MyDb db;
        private readonly IWebHostEnvironment _host;

        public FIT_PONG.Services.BL.iEmailServis emailServis { get; }

        public ReportController(FIT_PONG.Database.MyDb instanca,IWebHostEnvironment _webhost,
            FIT_PONG.Services.BL.iEmailServis imejlovi)
        {
            db = instanca;
            _host = _webhost;
            emailServis = imejlovi;
        }
        public IActionResult Dodaj()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Dodaj(CreateReportVM ReportObj)
        {
            if(ModelState.IsValid)
            {
                if (!SamoSlike(ReportObj.Prilozi))
                {
                    ModelState.AddModelError(nameof(ReportObj.Prilozi), "Mozete samo slike upload");
                }
                else
                {
                    using (var transakcija = db.Database.BeginTransaction())
                    {
                        List<string> zluNetrebalo = new List<string>();
                        try
                        {
                            //otkriti nacin slanja mejla administratoru ako uspjesno prodje report
                            Report noviReport = new Report
                            {
                                Naslov = ReportObj.Naslov,
                                Email = ReportObj.Email,
                                Sadrzaj = ReportObj.Sadrzaj,
                                DatumKreiranja = DateTime.Now,
                                Prilozi = new List<Attachment>()
                            };
                            db.Reports.Add(noviReport);
                            db.SaveChanges();
                            if (ReportObj.Prilozi != null)
                            {
                                foreach (IFormFile x in ReportObj.Prilozi)
                                {

                                    Directory.CreateDirectory(Path.Combine(_host.WebRootPath, "reports").ToString());
                                    string ImeFajla = Guid.NewGuid().ToString() + "_" + x.FileName;
                                    string PathSpremanja = Path.Combine(_host.WebRootPath, "reports", ImeFajla);
                                    using(var strim = new FileStream(PathSpremanja, FileMode.Create))
                                        x.CopyTo(strim);
                                    zluNetrebalo.Add(PathSpremanja);
                                    Attachment Attachmentnovi = new Attachment
                                    {
                                        DatumUnosa = DateTime.Now,
                                        Path = "~/reports/" + ImeFajla
                                    };
                                    db.Attachments.Add(Attachmentnovi);
                                    db.SaveChanges();
                                    noviReport.Prilozi.Add(Attachmentnovi);
                                    db.SaveChanges();

                                }
                            }
                            transakcija.Commit();
                            try
                            {
                               emailServis.PosaljiMejlReport(noviReport);
                            }
                            catch(Exception err)
                            {

                            }
                            return Redirect("/Home/Index");
                        }
                        catch(DbUpdateException err)
                        {
                            transakcija.Rollback();
                            foreach(string x in zluNetrebalo)
                            {
                                if (System.IO.File.Exists(x))
                                    System.IO.File.Delete(x);
                            }
                            ModelState.AddModelError("", "Nesto je krenulo po zlu prilikom spasavanja u bazu,ponovite unos");
                        }
                    }
                }
            }
            return View(ReportObj);
        }
        //public void PosaljiMejl(Report novi)
        //{
        //    var Poruka = new MimeMessage();
        //    Poruka.From.Add(new MailboxAddress("fitpongtest@gmail.com"));
        //    Poruka.To.Add(new MailboxAddress("nedim.kurtovic@edu.fit.ba"));
        //    //Poruka.To.Add(new MailboxAddress("aldin.talic@edu.fit.ba"));
        //    Poruka.Subject = novi.Naslov;
        //    Poruka.Body = new TextPart("html")
        //    {
        //        Text = "Poruka dolazi od " + novi.Email + "<br>" +
        //        "Sadrzaj poruke : " + novi.Sadrzaj
        //    };
        //    using(var client = new SmtpClient())
        //    {
        //        client.Connect("smtp.gmail.com", 587);
        //        client.Authenticate("fitpongtest@gmail.com", "F!tP0ng_2019!");
        //        client.Send(Poruka);
        //        client.Disconnect(false);
        //    }
        //}
        public bool SamoSlike(List<IFormFile> prilozi)
        {
            if (prilozi != null)
            {
                foreach (IFormFile x in prilozi)
                {
                    if (!x.ContentType.Contains("image"))
                        return false;
                }
            }
            return true;
        }

        public IActionResult Prikaz(int ? id)
        {
            if (id == null)
                return RedirectToAction("Neuspjeh");

            Report model = db.Reports.Include(x => x.Prilozi).SingleOrDefault(c=> c.ID == id);
            if (model != null)
                return View(model);
            return RedirectToAction("Neuspjeh");
        }
        public IActionResult Neuspjeh()
        {
            return View();
        }
    }
}