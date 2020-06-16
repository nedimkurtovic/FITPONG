using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FIT_PONG.WebAPI.Services.Bazni;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CRUDController<T, TInsert, TUpdate, TDb, TSearch> : BaseController<T, TSearch>
    {
        private readonly ICRUDService<T, TInsert, TUpdate, TSearch> CRUDservis;

        public CRUDController(ICRUDService<T, TInsert, TUpdate, TSearch> _servis) : base(_servis)
        {
            CRUDservis = _servis;
        }
        [HttpPost]
        public T Insert(TInsert obj)
        {
            return CRUDservis.Add(obj);
        }
        [HttpPut("{id}")]
        public T Update(int id, [FromBody] TUpdate obj)
        {
            return CRUDservis.Update(id,obj);
        }
        [HttpDelete("{id}")]
        public string Delete(int id)
        {
            CRUDservis.Delete(id);
            return "Radnja obavljena";           
        }
    }
}