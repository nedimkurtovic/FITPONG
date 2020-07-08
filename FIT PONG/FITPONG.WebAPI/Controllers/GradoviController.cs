using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class GradoviController : ControllerBase
    {
        private readonly IGradoviService gradoviService;

        public GradoviController(IGradoviService gradoviService)
        {
            this.gradoviService = gradoviService;
        }

        [HttpGet]
        public List<Gradovi> Get()
        {
            return gradoviService.Get();
        } 
    }
}