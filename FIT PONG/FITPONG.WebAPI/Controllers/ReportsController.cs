using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.Services.Services;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Reports;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    public class ReportsController : ControllerBase
    {
        private readonly IReportsService reportsService;

        public ReportsController(IReportsService reportsService)
        {
            this.reportsService = reportsService;
        }


        [HttpGet]
        public List<Reports> Get()
        {
            return reportsService.Get();
        }

        [HttpGet("{id}")]
        public Reports Get(int id)
        {
            return reportsService.GetById(id);
        }

        [HttpPost]
        public Reports Add(ReportsInsert obj)
        {
            return reportsService.Add(obj,"~"); 
        }

    }
}