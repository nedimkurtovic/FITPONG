using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FIT_PONG.Services.Services;
using FIT_PONG.Services.Services.Autorizacija;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace FIT_PONG.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize(AuthenticationSchemes = "BasicAuthentication")] 
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IStatistikeService statistikeService;
        private readonly IUsersAutorizator usersAutorizator;

        public UsersController(IUsersService usersService, IStatistikeService statistikeService, IUsersAutorizator usersAutorizator)
        {
            this.usersService = usersService;
            this.statistikeService = statistikeService;
            this.usersAutorizator = usersAutorizator;
        }

        [Authorize(AuthenticationSchemes = "BasicAuthentication")] 
        [HttpGet]
        public PagedResponse<Users> Get([FromQuery]AccountSearchRequest obj)
        {
            var respons = GetPagedResponse(obj);
            return respons;
        }

        [Authorize(AuthenticationSchemes = "BasicAuthentication")] 
        [HttpGet("{id}")]
        public Users Get(int id)
        {
            return usersService.Get(id);
        }


        [HttpPost]
        [Route("registracija")]
        public async Task<Users> Register(AccountInsert obj)
        {
            return await usersService.Register(obj);
        }


        [HttpPost]
        [Route("login")]
        public async Task<Users> Login(Login obj)
        {
            return await usersService.Login(obj);
        }


        [HttpPut("{id}")]
        [AllowAnonymous]
        public Users Edit(int id, AccountUpdate obj)
        {
            usersAutorizator.AuthorizeEditProfila(usersService.GetRequestUserID(HttpContext.Request), id);
            return usersService.EditujProfil(id,obj);
        }


        [HttpPost]
        [Route("mailPonovnoSlanje")]
        public async Task<string> PonovoPosaljiMail(Email_Password_Request obj)
        {
            return await usersService.SendConfirmationEmail(obj);
        }


        [HttpGet]
        [Route("potvrdiMejl")]
        public async Task<String> ConfirmEmail([FromQuery]string userId, [FromQuery]string token)
        {
            return await usersService.ConfirmEmail(userId, token);
        }


        [HttpPost]
        [Route("promjenaPassworda")]
        public async Task<string> ResetujPassword(Email_Password_Request obj)
        {
            return await usersService.SendPasswordChange(obj);
        }

        [HttpPost]
        [Route("potvrdiPromjenuPassworda")]
        public async Task<String> ConfirmPasswordChange(PasswordPromjena obj)
        {
            var loggedInUserName = usersService.GetRequestUserName(HttpContext.Request);

            return await usersService.ConfirmPasswordChange(loggedInUserName, obj);
        }

        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpPost]
        [Route("{postovaniId}/akcije/postovanje")]
        public string Postovanje(int postovaniId)
        {
            var loggedInUserName = usersService.GetRequestUserName(HttpContext.Request);

            return usersService.Postovanje(loggedInUserName, postovaniId);
        }

        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpPost]
        [Route("{userId}/akcije/promijeniSliku")]
        public string PromijeniSliku(int userId, SlikaPromjenaRequest obj)
        {
            var loggedInUserName = usersService.GetRequestUserName(HttpContext.Request);

            var fajl = new Fajl
            {
                BinarniZapis = obj.Slika,
                Naziv=obj.Naziv
            };

            return usersService.UpdateProfilePicture(loggedInUserName, userId, fajl);
        }

        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpPost]
        [Route("{userId}/akcije/ukloniSliku")]
        public string UkloniSliku(int userId)
        {
            var loggedInUserName = usersService.GetRequestUserName(HttpContext.Request);

            return usersService.ResetProfilePicture(loggedInUserName, userId);
        }

        [Authorize(AuthenticationSchemes = "BasicAuthentication")]
        [HttpGet("{id}/statistike")]
        public List<Statistike> Statistike(int id)
        {
            return statistikeService.Get(id);
        }
        private PagedResponse<Users> GetPagedResponse(AccountSearchRequest obj)
        {
            var listaUsera = usersService.Get(obj);
            PagedResponse<Users> respons = new PagedResponse<Users>();

            respons.TotalPageCount = (int)Math.Ceiling((double)listaUsera.Count() / (double)obj.Limit);
            respons.Stavke = listaUsera.Skip((obj.Page - 1) * obj.Limit).Take(obj.Limit).ToList();

            AccountSearchRequest iducaKlon = obj.Clone() as AccountSearchRequest;
            iducaKlon.Page = (iducaKlon.Page + 1) > respons.TotalPageCount ? -1 : iducaKlon.Page + 1;
            String iduciUrl = iducaKlon.Page == -1 ? null : this.Url.Action("Get", null, iducaKlon, Request.Scheme);

            AccountSearchRequest proslaKlon = obj.Clone() as AccountSearchRequest;
            proslaKlon.Page = (proslaKlon.Page - 1) < 0 ? -1 : proslaKlon.Page - 1;
            String prosliUrl = proslaKlon.Page == -1 ? null : this.Url.Action("Get", null, proslaKlon, Request.Scheme);

            respons.IducaStranica = !String.IsNullOrWhiteSpace(iduciUrl) ? new Uri(iduciUrl) : null;
            respons.ProslaStranica = !String.IsNullOrWhiteSpace(prosliUrl) ? new Uri(prosliUrl) : null;
            return respons;
        }
    }
}