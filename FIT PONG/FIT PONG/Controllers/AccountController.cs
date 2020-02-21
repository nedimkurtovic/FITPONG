using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;
using System.Threading.Tasks;
using FIT_PONG.Models;
using FIT_PONG.Models.BL;
using FIT_PONG.ViewModels.AccountVMs;
using FIT_PONG.ViewModels.IgracVMs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace FIT_PONG.Controllers
{
    public class AccountController : Controller
    {
        private readonly MyDb db;
        private readonly SignInManager<IdentityUser<int>> SignIn;
        private readonly UserManager<IdentityUser<int>> UserM;
        public iEmailServis EmailServis { get; }

        public AccountController(MyDb instanca,
            SignInManager<IdentityUser<int>> menadzer,
            UserManager<IdentityUser<int>> menadzerusera,
            iEmailServis emailServis
            )
        {
            db = instanca;
            SignIn = menadzer;
            UserM = menadzerusera;
            EmailServis = emailServis;
        }

        
        [HttpGet]
        [AllowAnonymous]

        public IActionResult Registracija()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Registracija(RegistracijaVM obj)
        {
            if(ModelState.IsValid)
            {
                IdentityUser<int> novi = new IdentityUser<int>
                {
                    UserName = obj.Email,
                    Email = obj.Email
                };
                var rezult = await UserM.CreateAsync(novi, obj.Password);
                if(rezult.Succeeded)
                {
                    //await SignIn.SignInAsync(novi, false);
                    //return RedirectToAction("Index", "Home");

                    var tokenko = await UserM.GenerateEmailConfirmationTokenAsync(novi);
                    string url = Url.Action("PotvrdiMail", "Account", new { userid = novi.Id, token = tokenko }, Request.Scheme);
                    EmailServis.PosaljiKonfirmacijskiMejl(url, novi.Email);
                    return RedirectToAction("UspjesnaRegistracija");
                }
                else
                {
                    foreach(var i in rezult.Errors)
                    {
                        ModelState.AddModelError("", i.Description);
                    }
                }
            }
            return View(obj);
        }
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> PotvrdiMail(string userid, string token)
        {
            if (userid == null || token == null)
                return RedirectToAction("Index", "Home");
            var user = await UserM.FindByIdAsync(userid);
            if (user == null)
            {
                ViewBag.Poruka = "Ne postoji taj user";
                return View("Neuspjeh");
            }
            var igrac = db.Igraci.Where(x => x.ID == user.Id).FirstOrDefault();
            if (igrac == null)
            {
                return RedirectToAction("Dodaj","Igrac", new { id = (int.Parse(userid)), token = token });
            }
            db.ChangeTracker.Entries().ToList();//visak sam ozbog testiranja stavljeno
            var rezultatko = await UserM.ConfirmEmailAsync(user, token);
            if(rezultatko.Succeeded)
            {
                return View();
            }
            ViewBag.Poruka = "Doslo je do greske prilikom potvrde mejla";
            return View("Neuspjeh");

        }
        public async Task<IActionResult> Logout()
        {
            await SignIn.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        [AllowAnonymous]

        public async Task<IActionResult> Login(LoginVM obj)
        {
            if(ModelState.IsValid)
            {
                var korisnik = await UserM.FindByEmailAsync(obj.UserName);
                var rezultat = await SignIn.PasswordSignInAsync(obj.UserName,obj.Password,obj.RememberMe,false);
                if(rezultat.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                else if(await SignIn.UserManager.CheckPasswordAsync(korisnik,obj.Password))
                {
                    ModelState.AddModelError("", "Morate potvrditi mejl prije logiranja");
                }
                else
                {
                    ModelState.AddModelError("", "Korisnik ne postoji");
                }
            }
            return View(obj);
        }
        [AllowAnonymous]
        public IActionResult UspjesnaRegistracija()
        {
            return View();
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PromjenaRequest()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PromjenaRequest(ResetPasswordEmailVM obj)
        {
            if(ModelState.IsValid)
            {
                var user = await UserM.FindByEmailAsync(obj.Email);
                if(user != null && user.EmailConfirmed)
                {
                    var token = await UserM.GeneratePasswordResetTokenAsync(user);
                    string url = Url.Action("PromjenaPassworda", "Account", new
                    {
                        user = user.Email,
                        token = token
                    }, Request.Scheme);
                    EmailServis.PosaljiResetPassword(url, obj.Email);
                }
                return View("ZahtjevPoslan");
            }
            return View(obj);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PromjenaPassworda(string user, string token)
        {
            ViewBag.email = user;
            ViewBag.token = token;
            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PromjenaPassworda(ResetPasswordVM obj)
        {
            if(ModelState.IsValid)
            {
                var user = await UserM.FindByEmailAsync(obj.email);
                if(user != null)
                {
                    var rezultat = await UserM.ResetPasswordAsync(user, obj.token, obj.password);
                    if (rezultat.Succeeded)
                        return View("UspjesnaPromjena");
                    else
                    {
                        foreach (var error in rezultat.Errors)
                            ModelState.AddModelError("", error.Description);
                        ViewBag.email = obj.email;
                        ViewBag.token = obj.token;
                        return View(obj);
                    }

                }
                return View("UspjesnaPromjena");
            }
            ViewBag.email = obj.email;
            ViewBag.token = obj.token;
            return View(obj);
        }
    }
}