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
using Google.Authenticator;
using System.Text;
using Microsoft.AspNetCore.Mvc.Rendering;

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
                if (korisnik == null)
                {
                    ModelState.AddModelError("", "Neispravni podaci za login.");
                    return View(obj);
                }
                var rezultat = await SignIn.PasswordSignInAsync(obj.UserName,obj.Password,obj.RememberMe,false);
                var Igrac = db.Igraci.Find(korisnik.Id);

                if (rezultat.IsLockedOut)
                {
                    TimeSpan t = (korisnik.LockoutEnd - DateTime.Now) ?? default(TimeSpan);
                    ModelState.AddModelError("Lockout", "Vaš profil je zaključan još " + t.Minutes + " minuta i " + t.Seconds + " sekundi.");
                }
                else if (rezultat.Succeeded)
                {
                    if (Igrac.TwoFactorEnabled)
                    {
                        await SignIn.SignOutAsync();

                        TempData["username"] = obj.UserName;
                        TempData["password"] = obj.Password;
                        TempData["rememberMe"] = obj.RememberMe;
                        return RedirectToAction("ProvjeriAutentifikaciju");
                    }
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
            ViewBag.poruka = "Za resetovanje passworda unesite mejl";
            ViewBag.akcija = "PromjenaRequest";
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
                    try
                    {
                        EmailServis.PosaljiResetPassword(url, obj.Email);
                        return View("ZahtjevPoslan");
                    }
                    catch (Exception err)
                    {
                        ModelState.AddModelError("", @"Doslo je do greske prilikom slanja mejla, 
                                                ukoliko se problem ponovi obavijestite administratora");
                    }
                }
                return View("ZahtjevPoslan");
            }
            return View(obj);
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult PosaljiKonfirmacijskiMejlPonovo()
        {
            ViewBag.poruka = "Unesite vasu email adresu";
            ViewBag.akcija = "PosaljiKonfirmacijskiMejlPonovo";
            return View("PromjenaRequest");
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> PosaljiKonfirmacijskiMejlPonovo(ResetPasswordEmailVM obj)
        {
            if (ModelState.IsValid)
            {
                var user = await UserM.FindByEmailAsync(obj.Email);
                if (user != null && !user.EmailConfirmed)
                {
                    var tokenko = await UserM.GenerateEmailConfirmationTokenAsync(user);
                    string url = Url.Action("PotvrdiMail", "Account", new { userid = user.Id, token = tokenko }, Request.Scheme);
                    try
                    {
                        EmailServis.PosaljiKonfirmacijskiMejl(url, obj.Email);
                        return View("ZahtjevPoslan");

                    }
                    catch (Exception err)
                    {
                        ModelState.AddModelError("",@"Doslo je do greske prilikom slanja mejla, pokusajte ponovo, 
                                                ukoliko se problem ponovi obavijestite administratora ,"+err.Message);
                        return View(obj);
                    }
                }
            }
            //ovo su mjere zastite
            return View("ZahtjevPoslan");
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


        [AllowAnonymous]
        public ActionResult ProvjeriAutentifikaciju()
        {
            if (TempData["username"].ToString() != null)
            {
                TempData.Keep("username");

                return View();
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<ActionResult> ProvjeriAutentifikaciju(AutentifikacijaVM model)
        {
            var korisnik = await UserM.FindByEmailAsync(TempData["username"].ToString());
            TempData.Keep("username");
            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string userUniqueKey = GetUserUniqueKey(korisnik);
            var token = model.Code;
            bool isValid = tfa.ValidateTwoFactorPIN(userUniqueKey, token);
            if (LockoutCheck(korisnik))
            {
                TimeSpan t = (korisnik.LockoutEnd - DateTime.Now)??default(TimeSpan);
                ModelState.AddModelError("Lockout", "Vaš profil je zaključan još " + t.Minutes + " minuta i "+t.Seconds +" sekundi.");
                return View(model);
            }
            else
            {
                if (TempData["code"] != null)
                {
                    if (TempData["code"].ToString() == token)
                    {
                        isValid = true;
                        TempData["code"] = null;
                    }
                    else
                        TempData.Keep("code");
                }

                if (isValid)
                {
                    await SignIn.PasswordSignInAsync(TempData["username"].ToString(), TempData["password"].ToString(),
                            Convert.ToBoolean(TempData["rememberMe"]), false);
                    TempData["code"] = null;

                    return Redirect("/Igrac/PrikazProfila/" + korisnik.Id);
                }
                else
                {
                    ModelState.AddModelError("Code", "Neispravan kod");
                    return View(model);
                }
            }
        }

        [HttpGet]
        public async Task<IActionResult> UkljuciAutentifikaciju()
        {
            var korisnik = await UserM.FindByEmailAsync(User.Identity.Name);
            Igrac i = db.Igraci.Find(korisnik.Id);

            if (!i.TwoFactorEnabled)
            {
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                byte[] byteUniqueKey = Encoding.ASCII.GetBytes(GetUserUniqueKey(korisnik));
                var setupInfo = tfa.GenerateSetupCode("FIT PONG", User.Identity.Name, byteUniqueKey, 2);
                ViewBag.qrcode = setupInfo.QrCodeSetupImageUrl;
                return View();
            }

            return Redirect("/Igrac/PrikazProfila/" + i.ID);
        }

        [HttpPost]
        public async Task<ActionResult> UkljuciAutentifikaciju(AutentifikacijaVM model)
        {
            if (User.Identity.Name != null)
            {
                var korisnik = await UserM.FindByEmailAsync(User.Identity.Name);
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                string userUniqueKey = GetUserUniqueKey(korisnik);

                if (LockoutCheck(korisnik))
                {
                    TimeSpan t = (korisnik.LockoutEnd - DateTime.Now) ?? default(TimeSpan);
                    ModelState.AddModelError("Lockout", "Vaš profil je zaključan još " + t.Minutes + " minuta i " + t.Seconds + " sekundi.");
                    return View();
                }
                else
                {
                    if (tfa.ValidateTwoFactorPIN(userUniqueKey, model.Code))
                    {
                        Igrac i = db.Igraci.Find(korisnik.Id);
                        i.TwoFactorEnabled = true;
                        db.Update(i);
                        db.SaveChanges();
                        return Redirect("/Igrac/PrikazProfila/" + i.ID);
                    }
                    else
                    {
                        ModelState.AddModelError("Code", "Neispravan kod");
                        return View();
                    }
                }
            }
            else
            {
                return RedirectToAction("Login");
            }
        }

        [HttpGet]
        public async Task<IActionResult> IskljuciAutentifikaciju()
        {
            var korisnik = await UserM.FindByEmailAsync(User.Identity.Name);
            Igrac i = db.Igraci.Find(korisnik.Id);

            if (i.TwoFactorEnabled)
                return View();

            return Redirect("/Igrac/PrikazProfila/" + i.ID);
        }

        [HttpPost]
        public async Task<ActionResult> IskljuciAutentifikaciju(AutentifikacijaVM model)
        {
            if (User.Identity.Name != null)
            {
                var korisnik = await UserM.FindByEmailAsync(User.Identity.Name);
                string userUniqueKey = GetUserUniqueKey(korisnik);
                TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
                if (LockoutCheck(korisnik))
                {
                    TimeSpan t = (korisnik.LockoutEnd - DateTime.Now) ?? default(TimeSpan);
                    ModelState.AddModelError("Lockout", "Vaš profil je zaključan još " + t.Minutes + " minuta i " + t.Seconds + " sekundi.");
                    return View();
                }
                else
                {
                    if (tfa.ValidateTwoFactorPIN(userUniqueKey, model.Code))
                    {
                        Igrac i = db.Igraci.Find(korisnik.Id);
                        i.TwoFactorEnabled = false;
                        db.Update(i);
                        db.SaveChanges();
                        return Redirect("/Igrac/PrikazProfila/" + i.ID);
                    }
                    else
                    {
                        ModelState.AddModelError("Code", "Neispravan kod");
                        return View();
                    }
                }
            }
            return RedirectToAction("Login");
        }

        [AllowAnonymous]
        public IActionResult PosaljiMail()
        {
            Random rand = new Random();
            var code = rand.Next(9999999);
            EmailServis.PosaljiTwoFactorCode(code, TempData["username"].ToString());
            TempData.Keep("username");
            TempData["code"] = code;
            return Redirect("ProvjeriAutentifikaciju");
        }

        private string GetUserUniqueKey(IdentityUser<int> korisnik)
        {
            string key = korisnik.SecurityStamp.Substring(5, 10);
            string useruniquekey = korisnik.Email + key;
            return useruniquekey;
        }

        private bool LockoutCheck(IdentityUser<int> korisnik)
        {
            if (korisnik.LockoutEnd == null)
                return false;
            if (korisnik.LockoutEnd < DateTime.Now)
            {
                if (korisnik.AccessFailedCount < 2)
                {
                    korisnik.AccessFailedCount++;
                    db.Update(korisnik);
                    db.SaveChanges();
                    return false;
                }
                korisnik.LockoutEnd = DateTime.Now.AddMinutes(1);
                korisnik.AccessFailedCount = 0;
                db.Update(korisnik);
                db.SaveChanges();
                return true;
            }
            return true;
        }
    }
}