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
            var host = $"http://{ HttpContext.Request.Host }";

            return usersService.Register(obj, host.ToString());
        }

        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        public Task<Users> Login(Login obj)
        {
            return usersService.Login(obj);
        }

        [HttpGet]
        [Route("/potvrdiMejl")]
        [AllowAnonymous]
        public Task<String> ConfirmEmail([FromQuery]string userId, [FromQuery]string token)
        {
            return usersService.ConfirmEmail(userId, token);
        }

        [HttpPost]
        [Route("{postovaniId}/akcije/postuj")]
        public string Postovanje(int postovaniId)
        {
            //var loggedInUserName = User.Identity.Name;  

            var loggedInUserName = "testni1";

            return usersService.Postovanje(loggedInUserName, postovaniId);
        }
    }
}