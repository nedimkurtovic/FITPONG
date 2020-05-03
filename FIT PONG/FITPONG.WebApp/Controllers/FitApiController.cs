using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using FIT_PONG.Models;
using FIT_PONG.Models.BL;
using FIT_PONG.ViewModels.TakmicenjeVMs;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoreLinq;
using Newtonsoft.Json;
using FIT_PONG.Database.DTOs;

namespace FIT_PONG.Controllers
{
    public class FitApiController : ApiController
    {
        private readonly MyDb db;
        private readonly UserManager<IdentityUser<int>> userManager;
        private readonly SignInManager<IdentityUser<int>> signInManager;
        private readonly IHttpContextAccessor httpContextAccessor;
        private readonly Evidentor evidentor;

        public FitApiController(MyDb db, 
            UserManager<IdentityUser<int>> userManager,
            SignInManager<IdentityUser<int>> signInManager,
            IHttpContextAccessor httpContextAccessor,
            Evidentor evidentor)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.httpContextAccessor = httpContextAccessor;
            this.evidentor = evidentor;
        }

        
        public string Get(int id)
        {
            Takmicenje obj = db.Takmicenja.Where(x => x.ID == id).FirstOrDefault();
            if (obj == null)
            {
                return "";
            }
            List<(string tim1, int? rez1, int? rez2, string tim2)> parovi = new List<(string tim1, int? set1, int? set2, string tim2)>();
            List<Utakmica> sveNaTakmicenju = db.Utakmice.AsNoTracking()
                .Include(x => x.UcescaNaUtakmici)
                .Include(x => x.Runda).ThenInclude(x => x.Bracket).ThenInclude(x => x.Takmicenje)
                .Where(x => x.Runda.Bracket.TakmicenjeID == id).ToList();

            foreach (Utakmica i in sveNaTakmicenju)
            {
                (string tim1, int? rez1, int? rez2, string tim2) par = evidentor.GetPar(i, id);
                parovi.Add(par);
            }
            var jsonObj = JsonConvert.SerializeObject(parovi);
            return jsonObj;
        }

        public string GetIgrace()
        {
            return JsonConvert.SerializeObject(db.Igraci.Select(x => new { id = x.ID, name = x.PrikaznoIme, avatar = x.ProfileImagePath, type = "contact" }).ToList());
        }

    }
}