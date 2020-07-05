using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using FIT_PONG.Services.Services;
using FIT_PONG.Services.Services.Autorizacija;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //authorize ce ici na nivou citavog kontrolera, zasebne stavke ce imati vlastite auth atribute
    public class TakmicenjaController : ControllerBase
    {
        private readonly ITakmicenjeService takmicenjeService;
        private readonly IUsersService usersService;
        private readonly ITakmicenjeAutorizator takmicenjeAutorizator;

        //treba razdvojiti logiku, tj valjalo bi imati TakmicenjeAutorizacijaService, koji ce se iskljucivo
        //baviti autorizacijom, nema smisla, bukvalno u 90% slucajeva trazi se od takmicenje servisa da se
        //brine oko autorizacije
        public TakmicenjaController(ITakmicenjeService _takmicenjeService, IUsersService _usersService,
            ITakmicenjeAutorizator _takmicenjeAutorizator)
        {
            takmicenjeService = _takmicenjeService;
            usersService = _usersService;
            takmicenjeAutorizator = _takmicenjeAutorizator;
        }

        [HttpGet]
        public List<Takmicenja> Get([FromQuery]TakmicenjeSearch obj)
        {
            return takmicenjeService.Get(obj);
        }
        [HttpGet("{id}")]
        public Takmicenja Get(int id)
        {
            return takmicenjeService.GetByID(id);
        }

        [HttpPost]
        public Takmicenja Insert(TakmicenjaInsert obj)
        {
            //var userName = usersService.GetUsername(HttpContext.Request.Headers); nesto na ovaj fazon
            //takmicenjeService.Add(obj, userName); nesto na ovaj fazon
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        //ovdje ce biti potrebno authorizovati
        public Takmicenja Update(int id, TakmicenjaUpdate obj)
        {
            //var UserID = usersService.GetUserID(HttpContext.Request.Headers);
            //return takmicenjeService.Update(id, obj, UserID); nesto ovako
            throw new NotImplementedException();

        }
        [HttpDelete("{id}")]
        public Takmicenja Delete(int id)
        {
            //var UserID = usersService.GetUserID(HttpContext.Request.Headers);
            //return takmicenjeService.Delete(id, UserID); nesto ovako
            throw new NotImplementedException();
        }

        [HttpPost("{id}/akcije/init")]
        public Takmicenja Init(int id)
        {
            //var UserID = usersService.GetUserID(HttpContext.Request.Headers);
            //return takmicenjeService.Init(id, UserID); nesto ovako
            throw new NotImplementedException();
        }

        [HttpGet("{id}/raspored")]
        public List<RasporedStavka> GetRaspored(int id)
        {
            return takmicenjeService.GetRaspored(id);
        }

        [HttpGet("{id}/evidencije")]
        public List<EvidencijaMeca> GetEvidencije(int id)
        {
            //var userName = usersService.GetUserName(HttpContext.Request.Headers);
            //return takmicenjeService.GetEvidencije(userName, id); nesto ovako
            throw new NotImplementedException();
        }

        [HttpPost("{id}/evidencije")]
        public List<EvidencijaMeca> EvidentirajMec(int id, [FromBody]EvidencijaMeca obj)
        {
            //var userName = usersService.GetUserName(HttpContext.Request.Headers);
            //return takmicenjeService.GetEvidencije(userName, id); nesto ovako
            throw new NotImplementedException();
        }
        
        [HttpGet("{id}/tabela")]
        public List<TabelaStavka> GetTabela(int id)
        {
            return takmicenjeService.GetTabela(id);
        }


        //nedostaje :
        //39.	POST 	/takmicenja/{id}/prijave	
        //43.	GET	/takmicenje/{id}/utakmice
        //47.	GET 	/takmicenje/{id}/favoriti

        //postoje sljedeci endpointi:
        /*
         * Statistike
            48.	GET	/statistike/{id}		//FIT_PONG.SharedModels.Statistike.cs	
                Prijave

            49.	GET	/prijave/{id}		//FIT_PONG.SharedModels.Prijave
            50.	DELETE	/prijave/{id}

         * Utakmice
            51.	GET	/utakmice/{id}		//FIT_PONG.SharedModels.Utakmice
            52.	GET 	/utakmice/{id}/ucesca	//FIT_PONG.SharedModels.Utakmice
            53.	POST	/utakmice/{id}/oznaciUtakmicu
            Ucesca
            54.	GET	/ucesca/{id}

        Nisam siguran da li ima smisla stavljati ih u zaseban kontroler tj praviti posebne kontrolere za 1,2 endpointa?
         */
    }
}