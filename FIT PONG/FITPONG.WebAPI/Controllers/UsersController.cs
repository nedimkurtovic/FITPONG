using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.OpenApi.Models;

namespace FIT_PONG.WebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")] 
    public class UsersController : Controller
    {
        private readonly IUsersService usersService;
        private readonly IStatistikeService statistikeService;

        public UsersController(IUsersService usersService, IStatistikeService statistikeService)
        {
            this.usersService = usersService;
            this.statistikeService = statistikeService;
        }


        [HttpGet]
        public List<Users> Get([FromQuery]AccountSearchRequest obj)
        {
            return usersService.Get(obj);
        }


        [HttpGet("{id}")]
        public Users Get(int id)
        {
            return usersService.Get(id);
        }


        [HttpPost]
        [AllowAnonymous]
        [Route("registracija")]
        public Task<Users> Register(AccountInsert obj)
        {
            return usersService.Register(obj);
        }


        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public Task<Users> Login(Login obj)
        {
            return usersService.Login(obj);
        }


        [HttpPost]
        [Route("mailPonovnoSlanje")]
        [AllowAnonymous]
        public Task<string> PonovoPosaljiMail(Email_Password_Request obj)
        {
            return usersService.SendConfirmationEmail(obj);
        }


        [HttpGet]
        [Route("potvrdiMejl")]
        [AllowAnonymous]
        public Task<String> ConfirmEmail([FromQuery]string userId, [FromQuery]string token)
        {
            return usersService.ConfirmEmail(userId, token);
        }


        [HttpPost]
        [Route("promjenaPassworda")]
        public Task<string> ResetujPassword(Email_Password_Request obj)
        {
            return usersService.SendPasswordChange(obj);
        }

        [HttpPost]
        [Route("potvrdiPromjenuPassworda")]
        public Task<String> ConfirmPasswordChange(PasswordPromjena obj)
        {
            var loggedInUserName = usersService.GetPrikaznoIme(HttpContext.Request);

            return usersService.ConfirmPasswordChange(loggedInUserName, obj);
        }


        [HttpPost]
        [Route("{postovaniId}/akcije/postovanje")]
        public string Postovanje(int postovaniId)
        {
            var loggedInUserName = usersService.GetPrikaznoIme(HttpContext.Request);

            return usersService.Postovanje(loggedInUserName, postovaniId);
        }


        [HttpPost]
        [Route("{userId}/akcije/promijeniSliku")]
        public string PromijeniSliku(int userId, SlikaPromjenaRequest obj)
        {
            var loggedInUserName = usersService.GetPrikaznoIme(HttpContext.Request);

            var fajl = new Fajl
            {
                BinarniZapis = obj.Slika,
                Naziv=obj.Naziv
            };

            return usersService.UpdateProfilePicture(loggedInUserName, userId, fajl);
        }


        [HttpPost]
        [Route("{userId}/akcije/ukloniSliku")]
        public string UkloniSliku(int userId)
        {
            var loggedInUserName = usersService.GetPrikaznoIme(HttpContext.Request);

            return usersService.ResetProfilePicture(loggedInUserName, userId);
        }

    }
}