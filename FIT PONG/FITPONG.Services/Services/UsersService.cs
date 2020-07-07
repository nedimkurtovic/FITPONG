using FIT_PONG.Database;
using FIT_PONG.Database.DTOs;
using FIT_PONG.Services.BL;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using System;
using System.Collections.Generic;
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
        public UsersService(MyDb _db, SignInManager<IdentityUser<int>> _singinmanager,
            UserManager<IdentityUser<int>> _usermanager, iEmailServis _mailservis)
        {
            db = _db;
            usermanager = _usermanager;
            signinmanager = _singinmanager;
            mailservis = _mailservis;
        }

        public Users ConfirmEmail(int id, string token)
        {
            throw new NotImplementedException();
        }

        public string ConfirmPasswordChange(int id, string token, string password)
        {
            throw new NotImplementedException();
        }

        public List<Users> Get(AccountSearchRequest obj)
        {
            throw new NotImplementedException();
        }

        public Users Get(int ID)
        {
            throw new NotImplementedException();
        }

        public Users Login(Login obj)
        {
            throw new NotImplementedException();
        }

        public string Logout(int id, string username)
        {
            throw new NotImplementedException();
        }

        public string Postovanje(int postivalacID, int postovaniID)
        {
            throw new NotImplementedException();
        }

        public Users Register(AccountInsert obj)
        {
            throw new NotImplementedException();
        }

        public string ResetProfilePicture(int id)
        {
            throw new NotImplementedException();
        }

        public string SendConfirmationEmail(Email_Password_Request obj)
        {
            throw new NotImplementedException();
        }

        public string SendPasswordChange(Email_Password_Request obj)
        {
            throw new NotImplementedException();
        }

        public string UpdateProfilePicture(int id, byte[] Slika)
        {
            throw new NotImplementedException();
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
