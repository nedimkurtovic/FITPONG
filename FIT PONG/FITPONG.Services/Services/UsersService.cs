using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.Services.BL;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using FITPONG.Services;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Google.Authenticator;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using System.Threading.Tasks;

namespace FIT_PONG.Services.Services
{
    public class UsersService : IUsersService
    {
        private readonly MyDb db;
        private readonly UserManager<IdentityUser<int>> usermanager;
        private readonly SignInManager<IdentityUser<int>> signinmanager;
        private readonly iEmailServis mailservis;
        private readonly Mapper mapper;
        public UsersService(MyDb _db, SignInManager<IdentityUser<int>> _singinmanager,
            UserManager<IdentityUser<int>> _usermanager, iEmailServis _mailservis, Mapper mapper)
        {
            db = _db;
            usermanager = _usermanager;
            signinmanager = _singinmanager;
            mailservis = _mailservis;
            this.mapper = mapper;
        }

        public async Task<Users> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                throw new UserException("UserID i token ne smiju biti null.");

            var user = await usermanager.FindByIdAsync(userId);
            if (user == null)
                throw new UserException("User ne postoji u bazi");

            var igrac = db.Igraci.Where(x => x.ID == user.Id).FirstOrDefault();
            if (igrac == null)
                throw new UserException("Igrac ne postoji u bazi.");

            var rezultat = await usermanager.ConfirmEmailAsync(user, token);
            if (!rezultat.Succeeded)
                throw new UserException("Doslo je do greske prilikom potvrde mejla.");

            return mapper.Map<SharedModels.Users>(user);
        }

        public string ConfirmPasswordChange(int id, string token, string password)
        {
            throw new NotImplementedException();
        }

        public List<Users> Get(AccountSearchRequest obj)
        {
            var users = new List<Database.DTOs.Igrac>();
            if (obj.PrikaznoIme == null)
                users = db.Igraci.ToList();
            else
                users = db.Igraci.Where(x => x.PrikaznoIme.Contains(obj.PrikaznoIme)).ToList();

            return mapper.Map<List<SharedModels.Users>>(users);
        }

        public Users Get(int ID)
        {
            var user = db.Igraci.Find(ID);
            if (user == null)
                throw new UserException("Korisnik ne postoji u bazi.");

            return mapper.Map<SharedModels.Users>(user);
        }

        public async Task<Users> Login(Login obj)
        {
            var korisnik = await usermanager.FindByEmailAsync(obj.UserName);
            if (korisnik == null)
                throw new UserException("Neispravni podaci za login.");

            var rezultat = await signinmanager.PasswordSignInAsync(obj.UserName, obj.Password, obj.RememberMe, false);
            var igrac = db.Igraci.Find(korisnik.Id);

            if (rezultat.IsLockedOut)
            {
                TimeSpan t = (korisnik.LockoutEnd - DateTime.Now) ?? default(TimeSpan);
                throw new Exception("Vaš profil je zaključan još " + t.Minutes + " minuta i " + t.Seconds + " sekundi.");
            }
            else if (rezultat.Succeeded)
            {
                //TO DO
                if (igrac.TwoFactorEnabled)
                {
                    await signinmanager.SignOutAsync();
                    return await ProvjeriAutentifikaciju(obj);
                }
            }
            else if (await signinmanager.UserManager.CheckPasswordAsync(korisnik, obj.Password))
            {
                throw new UserException("Morate potvrditi mejl prije logiranja");
            }
            else
            {
                throw new UserException("Korisnik ne postoji");
            }

            return mapper.Map<SharedModels.Users>(korisnik);
        }

        public async void Logout(int id, string username)
        {
            await signinmanager.SignOutAsync();
        }

        public string Postovanje(int postivalacID, int postovaniID)
        {
            var user1 = db.Igraci.Find(postivalacID);
            var user2 = db.Igraci.Find(postovaniID);

            if (user1 == null || user2 == null)
                throw new UserException("User ne postoji u bazi.");

            var postovanje = db.Postovanja.Where(p => p.PostivalacID == postivalacID && p.PostovaniID == postovaniID).SingleOrDefault();
            if (postovanje != null)
                db.Remove(postovanje);
            else
            {
                postovanje = new Database.DTOs.Postovanje(postivalacID, postovaniID);
                db.Add(postovanje);
            }

            db.SaveChanges();

            return "Postovanje uspjesno azurirano.";
        }

        public async Task<Users> Register(AccountInsert obj)
        {
            var user = new IdentityUser<int>
            {
                UserName = obj.Email,
                Email = obj.Email
            };

            var result = await usermanager.CreateAsync(user, obj.Password);
            if (result.Succeeded)
            {
                var token = await usermanager.GenerateEmailConfirmationTokenAsync(user);
                string url = $"/users/{user.Id}/ConfirmMail/{token}"; //provjeriti s Necom
                mailservis.PosaljiKonfirmacijskiMejl(url, user.Email);
                return mapper.Map<SharedModels.Users>(user);
            }
            else
            {
                var exception = new UserException();
                foreach (var error in result.Errors)
                {
                    exception.AddError(error.Code, error.Description);
                }
                throw exception;
            }

        }

        public string ResetProfilePicture(int id)
        {
            throw new NotImplementedException();
        }

        public string SendConfirmationEmail(Email_Password_Request obj)
        {
            throw new NotImplementedException();
        }

        public async Task<string> SendPasswordChange(Email_Password_Request obj)
        {
            var user = await usermanager.FindByEmailAsync(obj.Email);
            if (user != null && user.EmailConfirmed)
            {
                var token = await usermanager.GeneratePasswordResetTokenAsync(user);
                //string url = Url.Action("PromjenaPassworda", "Account", new
                //{
                //    user = user.Email,
                //    token = token
                //}, Request.Scheme);
                string url = "TO DO";

                try
                {
                    mailservis.PosaljiResetPassword(url, obj.Email);
                    return "Password uspjesno promijenjen.";
                }
                catch (Exception)
                {
                    //TO DO                
                }
            }
            throw new UserException("Korisnik ne postoji ili nije povrdio mail.");

        }

        public string UpdateProfilePicture(int id, byte[] Slika)
        {
            throw new NotImplementedException();
        }

        //*********************************************************
        //              POMOCNE FUNKCIJE                           
        //*********************************************************
        private async Task<Users> ProvjeriAutentifikaciju(Login obj)
        {
            var korisnik = await usermanager.FindByEmailAsync(obj.UserName);

            TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
            string userUniqueKey = GetUserUniqueKey(korisnik);
            var token = obj.Code;
            bool isValid = tfa.ValidateTwoFactorPIN(userUniqueKey, token);
            if (LockoutCheck(korisnik))
            {
                TimeSpan t = (korisnik.LockoutEnd - DateTime.Now) ?? default(TimeSpan);
                throw new UserException("Vaš profil je zaključan još " + t.Minutes + " minuta i " + t.Seconds + " sekundi.");
            }
            else
            {

                if (obj.Code != null && obj.Code == token)
                {
                    await signinmanager.PasswordSignInAsync(obj.UserName, obj.Password, obj.RememberMe, false);
                    return mapper.Map<SharedModels.Users>(korisnik);
                }
                else
                {
                    throw new UserException("Code je neispravan.");
                }
            }
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
