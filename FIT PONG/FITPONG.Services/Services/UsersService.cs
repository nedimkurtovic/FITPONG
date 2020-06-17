using AutoMapper;
using FIT_PONG.Database;
using FIT_PONG.Services.BL;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            var users=new List<Database.DTOs.Igrac>();
            if (obj.PrikaznoIme == null)
                users = db.Igraci.ToList();
            else
                users = db.Igraci.Where(x => x.PrikaznoIme.Contains(obj.PrikaznoIme)).ToList();

            return mapper.Map<List<SharedModels.Users>>(users);
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
    }
}
