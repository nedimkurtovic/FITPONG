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

            //autorizoatorove exceptione hvata glavni filterko babuka
            
            //Authorize politika tj basicauthentification handler ce rjesavati da li je ispravan
            //auth header kako bih ja mogao iz usersservice nesmetano samo pokupiti userid ili username
            //bez da se brinem da li ima gresaka u authorization headeru, dakle sve u basicauthhendleru 
            //se rjesava po tom pitanju

            //ALI Treba voditi ogromnog racuna o ovome : DA LI SE SALJE PRIKAZNOIME(Igrac) ILI USERNAME(User)
            //u auth headeru kao username, potrebno sto prije rijesiti tu dilemu
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
            var userId = usersService.GetUserID(HttpContext.Request);// nesto na ovaj fazon
            takmicenjeService.Add(obj, userId);// nesto na ovaj fazon
            throw new NotImplementedException();
        }

        [HttpPut("{id}")]
        //ovdje ce biti potrebno authorizovati
        public Takmicenja Update(int id, TakmicenjaUpdate obj)
        {
            var userId = usersService.GetUserID(HttpContext.Request);
            takmicenjeAutorizator.AuthorizeUpdate(userId, id);
            return takmicenjeService.Update(id, obj); 

        }
        [HttpDelete("{id}")]
        public Takmicenja Delete(int id)
        {
            var userId = usersService.GetUserID(HttpContext.Request);
            takmicenjeAutorizator.AuthorizeDelete(userId, id);
            return takmicenjeService.Delete(id);
        }

        [HttpPost("{id}/akcije/init")]
        public Takmicenja Init(int id)
        {
            var userId = usersService.GetUserID(HttpContext.Request);
            takmicenjeAutorizator.AuthorizeInit(userId, id);
            return takmicenjeService.Initialize(id);
        }

        [HttpGet("{id}/raspored")]
        public List<RasporedStavka> GetRaspored(int id)
        {
            return takmicenjeService.GetRaspored(id);
        }

        [HttpGet("{id}/evidencije")]
        public List<EvidencijaMeca> GetEvidencije(int id)
        {
            var userName = usersService.GetPrikaznoIme(HttpContext.Request);
            //ovdje treba biti oprezan ko kroz minsko polje zasto : 
                //u users servisu GetPrikaznoIme po trenutnoj implementaciji vraca PrikaznoIme
                //sto odgovara prvoj linij koda u GetEvidencije(dobavlja igraca na osnovu prikaznog imena)
                //u slucaju da GetPrikaznoIme vraca username iz usera(iako nema nikakve logike da to uradi)
                //bice belaj
            return takmicenjeService.GetEvidencije(userName, id);
        }

        [HttpPost("{id}/evidencije")]
        public List<EvidencijaMeca> EvidentirajMec(int id, [FromBody]EvidencijaMeca obj)
        {
            var userName = usersService.GetPrikaznoIme(HttpContext.Request);
            return takmicenjeService.GetEvidencije(userName, id);
        }
        
        [HttpGet("{id}/tabela")]
        public List<TabelaStavka> GetTabela(int id)
        {
            return takmicenjeService.GetTabela(id);
        }


        //nedostaje :
        //39.	POST 	/takmicenja/{id}/prijave	
        //43.	GET	/takmicenje/{id}/utakmice <- u kakav model spremit ovo? Ne koristi se nigdje 
        //tj nemamo nikakvu implementaciju koja ce koristiti ovo 
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