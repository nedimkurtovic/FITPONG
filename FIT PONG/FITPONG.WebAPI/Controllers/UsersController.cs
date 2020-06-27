using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        public List<Users> Get(AccountSearchRequest obj)
        {
            return usersService.Get(obj);
        }
        



    }
}