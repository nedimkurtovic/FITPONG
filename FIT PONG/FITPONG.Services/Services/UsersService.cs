﻿using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.Database.DTOs;
using FIT_PONG.Services.BL;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using FIT_PONG.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Google.Authenticator;
using ValidationContext = System.ComponentModel.DataAnnotations.ValidationContext;
using System.Threading.Tasks;
using System.Web;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading;

namespace FIT_PONG.Services.Services
{
    public class UsersService : IUsersService
    {
        private readonly MyDb db;
        private readonly UserManager<IdentityUser<int>> usermanager;
        private readonly SignInManager<IdentityUser<int>> signinmanager;
        private readonly iEmailServis mailservis;
        private readonly IMapper mapper;
        public UsersService(MyDb _db, SignInManager<IdentityUser<int>> _singinmanager,
            UserManager<IdentityUser<int>> _usermanager, iEmailServis _mailservis, IMapper mapper)
        {
            db = _db;
            usermanager = _usermanager;
            signinmanager = _singinmanager;
            mailservis = _mailservis;
            this.mapper = mapper;
        }

        public async Task<string> ConfirmEmail(string userId, string token)
        {
            if (userId == null || token == null)
                throw new UserException("UserID i token ne smiju biti null.");

            var user = await usermanager.FindByIdAsync(userId);
            if (user == null)
                throw new UserException("User ne postoji u bazi");


            var rezultat = await usermanager.ConfirmEmailAsync(user, token);
            if (!rezultat.Succeeded)
                throw new UserException("Doslo je do greske prilikom potvrde mejla.");

            return "Uspjesno ste potvrdili mail.";
        }

        public async Task<string> ConfirmPasswordChange(int id, string token, string password)
        {
            var u = db.Users.Find(id);

            if (u != null)
            {
                var user = await usermanager.FindByEmailAsync(u.Email);
                if (user != null)
                {
                    var rezultat = await usermanager.ResetPasswordAsync(user, token, password);
                    if (rezultat.Succeeded)
                        return "Password je uspjesno resetovan.";
                    else
                    {
                        UserException exception = new UserException();

                        foreach (var error in rezultat.Errors)
                            exception.AddError("", error.Description);

                        throw exception;
                    }
                }
            }
            throw new UserException("User ne postoji u bazi.");
        }

        public List<Users> Get(AccountSearchRequest obj)
        {
            var users = new List<Database.DTOs.Igrac>();
            if (obj.PrikaznoIme == null)
                users = db.Igraci.ToList();
            else
                users = db.Igraci.Where(x => x.PrikaznoIme.Contains(obj.PrikaznoIme)).ToList();

            var list = new List<SharedModels.Users>();

            foreach (var user in users)
            {
                var u = mapper.Map<SharedModels.Users>(user);
                u.listaPrijava = GetPrijave(user.ID);
                u.statistike = mapper.Map<List<SharedModels.Statistike>>(db.Statistike.Where(d => d.IgracID == user.ID).ToList());

                list.Add(u);
            }

            return list;
        }

        public Users Get(int ID)
        {
            var user = db.Igraci.Find(ID);
            if (user == null)
                throw new UserException("Korisnik ne postoji u bazi.");

            var u = mapper.Map<SharedModels.Users>(user);
            u.listaPrijava = GetPrijave(user.ID);
            u.statistike = mapper.Map<List<SharedModels.Statistike>>(db.Statistike.Where(d => d.IgracID == user.ID).ToList());

            return mapper.Map<SharedModels.Users>(u);
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
                return mapper.Map<SharedModels.Users>(igrac);
            }
            else if (await signinmanager.UserManager.CheckPasswordAsync(korisnik, obj.Password))
            {
                throw new UserException("Morate potvrditi mejl prije logiranja");
            }
            else
            {
                throw new UserException("Korisnik ne postoji");
            }

        }

        //OVO OSTAJE ZA RAZMISLJANJE...

        //public async void Logout(int id, string username)
        //{
        //    await signinmanager.SignOutAsync();
        //}

        public string Postovanje(string loggedInUserName, int postovaniID)
        {
            var user1 = db.Users.Where(d => d.Email == loggedInUserName).FirstOrDefault();
            var user2 = db.Igraci.Find(postovaniID);

            if (user1 == null || user2 == null)
                throw new UserException("User ne postoji u bazi.");

            int postivalacID = user1.Id;

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

        public async Task<Users> Register(AccountInsert obj, string host)
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
                mailservis.PosaljiKonfirmacijskiMejl(token, user.Email,"api");
                return mapper.Map<SharedModels.Users>(obj);
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

        public async Task<string> SendPasswordChange(Email_Password_Request obj, string host)
        {
            var user = await usermanager.FindByEmailAsync(obj.Email);
            if (user != null && user.EmailConfirmed)
            {
                var token = await usermanager.GeneratePasswordResetTokenAsync(user);

                try
                {
                    mailservis.PosaljiResetPassword(token, obj.Email, "api");
                    return "Poslan mail za promjenu passworda.";
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

        //private async Task<Users> ProvjeriAutentifikaciju(Login obj)
        //{
        //    var korisnik = await usermanager.FindByEmailAsync(obj.UserName);

        //    TwoFactorAuthenticator tfa = new TwoFactorAuthenticator();
        //    string userUniqueKey = GetUserUniqueKey(korisnik);
        //    var token = obj.Code;
        //    bool isValid = tfa.ValidateTwoFactorPIN(userUniqueKey, token);
        //    if (LockoutCheck(korisnik))
        //    {
        //        TimeSpan t = (korisnik.LockoutEnd - DateTime.Now) ?? default(TimeSpan);
        //        throw new UserException("Vaš profil je zaključan još " + t.Minutes + " minuta i " + t.Seconds + " sekundi.");
        //    }
        //    else
        //    {

        //        if (obj.Code != null && obj.Code == token)
        //        {
        //            await signinmanager.PasswordSignInAsync(obj.UserName, obj.Password, obj.RememberMe, false);
        //            return mapper.Map<SharedModels.Users>(korisnik);
        //        }
        //        else
        //        {
        //            throw new UserException("Code je neispravan.");
        //        }
        //    }
        //}

        //private string GetUserUniqueKey(IdentityUser<int> korisnik)
        //{
        //    string key = korisnik.SecurityStamp.Substring(5, 10);
        //    string useruniquekey = korisnik.Email + key;
        //    return useruniquekey;
        //}

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

        private List<Prijave> GetPrijave(int userId)
        {
            var prijaveIgraci = db.PrijaveIgraci.Include(p => p.Prijava).Where(d => d.IgracID == userId).ToList();
            var prijave = new List<SharedModels.Prijave>();

            foreach (var pi in prijaveIgraci)
            {
                var prijava = new SharedModels.Prijave
                {
                    ID = pi.PrijavaID,
                    Naziv = pi.Prijava.Naziv,
                    Igrac1ID = pi.IgracID,
                    isTim = false
                };

                if (pi.Prijava.isTim)
                {
                    prijava.isTim = true;
                    var prijava2 = db.PrijaveIgraci
                                        .Include(p => p.Prijava)
                                        .Where(p => p.Prijava.Naziv == pi.Prijava.Naziv && p.IgracID != pi.IgracID)
                                        .FirstOrDefault();

                    if (prijava2 != null)
                        prijava.Igrac2ID = prijava2.IgracID;
                }
                prijave.Add(prijava);
            }

            return prijave;
        }
        public int GetUserID(HttpRequest Request)
        {
            var credentials = GetCredentials(Request);
            var username = credentials[0];
            var igrac = GetIgraca(username);
            return igrac.ID;

        }
        public string GetPrikaznoIme(HttpRequest Request)
        {
            var credentials = GetCredentials(Request);
            var username = credentials[0];
            var igrac = GetIgraca(username);
            //potrebno prvojeriti da li ce se vracati rpikazno ime ili users.username i jos bitno:
            //potrebno je provjeriti da li ce se slati prikazno ime u authorization headeru
            //ili email, to sve zavisi od toga kako napravimo usera kad se kreira novi
            return igrac.PrikaznoIme;
        }
        private string[] GetCredentials(HttpRequest Request)
        {
            var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
            var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
            return credentials;
        }
        private Igrac GetIgraca(string username)
        {
            var user = db.Igraci.Where(x => x.PrikaznoIme == username).FirstOrDefault();
            if (user != null)
                return user;
            throw new UserException("User ne postoji ili je obrisan");
        }
    }
}
