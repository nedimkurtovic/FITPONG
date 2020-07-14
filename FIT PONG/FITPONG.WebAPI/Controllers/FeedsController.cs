using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Objave;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class FeedsController : ControllerBase
    {
        private readonly IFeedsService feedsService;
        private readonly IObjaveService objaveService;


        public FeedsController(IFeedsService feedsService, IObjaveService objaveService)
        {
            this.feedsService = feedsService;
            this.objaveService = objaveService;
        }

        [HttpGet]
        public List<Feeds> Get()
        {
            return feedsService.Get();
        }

        [HttpGet("{id}")]
        public Feeds Get(int id)
        {
            return feedsService.GetById(id);
        }


        [HttpPost("{id}/objave")]
        public Objave Add(int id, ObjaveInsertUpdate obj)
        {
            return objaveService.Add(id, obj);
        }   
    }
}