using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper.Configuration.Conventions;
using FIT_PONG.Services.Services;
using FIT_PONG.Services.Services.Autorizacija;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace FIT_PONG.WebAPI.Controllers
{
    [Route("api/[controller]")] 
    [ApiController]
    [Authorize(AuthenticationSchemes = "BasicAuthentication")]
    //authorize ce ici na nivou citavog kontrolera, zasebne stavke ce imati vlastite auth atribute
    public class TakmicenjaController : ControllerBase
    {
        private readonly ITakmicenjeService takmicenjeService;
        private readonly IUsersService usersService;
        private readonly IPrijaveService prijaveService;
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
            ITakmicenjeAutorizator _takmicenjeAutorizator, IPrijaveService prijaveService)
        {
            takmicenjeService = _takmicenjeService;
            usersService = _usersService;
            takmicenjeAutorizator = _takmicenjeAutorizator;
            this.prijaveService = prijaveService;
        }

        [HttpGet]
        public PagedResponse<Takmicenja> Get([FromQuery]TakmicenjeSearch obj)
        {
            var respons = GetPagedResponse(obj);
            return respons;
        }
        [HttpGet("{id}")]
        public Takmicenja Get(int id)
        {
            return takmicenjeService.GetByID(id);
        }

        [HttpPost]
        public Takmicenja Insert(TakmicenjaInsert obj)
        {
            var userId = usersService.GetRequestUserID(HttpContext.Request);// nesto na ovaj fazon
            // nesto na ovaj fazon
            return takmicenjeService.Add(obj, userId);
        }

        [HttpPatch("{id}")]
        //ovdje ce biti potrebno authorizovati
        public Takmicenja Update(int id, TakmicenjaUpdate obj)
        {
            var userId = usersService.GetRequestUserID(HttpContext.Request);
            takmicenjeAutorizator.AuthorizeUpdate(userId, id);
            return takmicenjeService.Update(id, obj); 

        }
        [HttpDelete("{id}")]
        public Takmicenja Delete(int id)
        {
            var userId = usersService.GetRequestUserID(HttpContext.Request);
            takmicenjeAutorizator.AuthorizeDelete(userId, id);
            return takmicenjeService.Delete(id);
        }

        [HttpPost("{id}/akcije/init")]
        public Takmicenja Init(int id)
        {
            var userId = usersService.GetRequestUserID(HttpContext.Request);
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
            var userName = usersService.GetRequestUserName(HttpContext.Request);
            //ovdje treba biti oprezan ko kroz minsko polje zasto : 
                //u users servisu GetPrikaznoIme po trenutnoj implementaciji vraca PrikaznoIme
                //sto odgovara prvoj linij koda u GetEvidencije(dobavlja igraca na osnovu prikaznog imena)
                //u slucaju da GetPrikaznoIme vraca username iz usera(iako nema nikakve logike da to uradi)
                //bice belaj

            //ovdje prije povratka treba nulirati igrace utakmica zbog problema sa json serijalizacijom,rekurzijom
            //i nemogucnosti limitiranja dubine json serijalizacije.
            //na web appu zbog ne koristenja jsona ostaje sve kako jest, tj prvenstveno ovo ovdje radim zato sto 
            //ne zelim mijenjati postojece stanje na webappu jer njemu trebaju list<igrac_utakmica> tim1 i tim2
            //zato cu tamo u evidentoru na evidenciji meca ponovo dobaviti List<Igrac_Utakmica> tim1 i tim2 ako su null
            var rezultat = takmicenjeService.GetEvidencije(userName, id);
            foreach(var i in rezultat)
            {
                i.Tim1 = null;
                i.Tim2 = null;
            }

            return rezultat;
        }

        [HttpPost("{id}/evidencije")]
        public EvidencijaMeca EvidentirajMec(int id, [FromBody]EvidencijaMeca obj)
        {
            var napunjeniObjekat = takmicenjeService.GetIgraceZaEvidenciju(obj, id);
            var userId = usersService.GetRequestUserID(HttpContext.Request);
            takmicenjeAutorizator.AuthorizeEvidencijaMeca(userId, napunjeniObjekat);
            var rezultat = takmicenjeService.EvidentirajMec(id, napunjeniObjekat);
            rezultat.Tim1 = null;
            rezultat.Tim2 = null;
            //i dalje je mozda upitno da li vracati sve moguce evidencije odma ili samo ovaj mec koji 
            //se upravo evidentirao
            //var userName = usersService.GetRequestUserName(HttpContext.Request);
            //return takmicenjeService.GetEvidencije(userName, id);
            return rezultat;
        }
        
        [HttpGet("{id}/tabela")]
        public List<TabelaStavka> GetTabela(int id)
        {
            return takmicenjeService.GetTabela(id);
        }


        [HttpGet("{id}/bloklista")]
        public List<Users> GetBlokLista(int id)
        {
            return takmicenjeService.GetBlokiraneIgrace(id);
        }


        [HttpPost("{id}/prijava/{prijavaid}/bloklista")]
        public Prijave BlokirajPrijavu(int id, int prijavaId)
        {
            return takmicenjeService.BlokirajPrijavu(id, prijavaId);
        }


        [HttpGet("{id}/prijave")]
        public List<Prijave> Prijava(int id)
        {
            return prijaveService.Get(id);
        }


        [HttpPost("{id}/prijava")]
        public Prijave Prijava(int id, PrijavaInsert obj)
        {
            var userId = usersService.GetRequestUserID(HttpContext.Request);
            takmicenjeAutorizator.AuthorizePrijava(userId, obj);
            return prijaveService.Add(id, obj);
        }


        private PagedResponse<Takmicenja> GetPagedResponse(TakmicenjeSearch obj)
        {
            var listaTakmicenja = takmicenjeService.Get(obj);
            PagedResponse<Takmicenja> respons = new PagedResponse<Takmicenja>
            {
                TotalPageCount = (int)Math.Ceiling((double)listaTakmicenja.Count() / (double)obj.Limit),
                Stavke = listaTakmicenja.Skip((obj.Page - 1) * obj.Limit).Take(obj.Limit).ToList()
            };

            TakmicenjeSearch iducaKlon = obj.Clone() as TakmicenjeSearch;
            iducaKlon.Page = (iducaKlon.Page + 1) > respons.TotalPageCount ? -1 : iducaKlon.Page + 1;
            String iduciUrl = iducaKlon.Page == -1 ? null : this.Url.Action("Get", null, iducaKlon, Request.Scheme);

            TakmicenjeSearch proslaKlon = obj.Clone() as TakmicenjeSearch;
            proslaKlon.Page = (proslaKlon.Page - 1) < 0 ? -1 : proslaKlon.Page - 1;
            String prosliUrl = proslaKlon.Page == -1 ? null : this.Url.Action("Get", null, proslaKlon, Request.Scheme);

            respons.IducaStranica = !String.IsNullOrWhiteSpace(iduciUrl) ? new Uri(iduciUrl) : null;
            respons.ProslaStranica = !String.IsNullOrWhiteSpace(prosliUrl) ? new Uri(prosliUrl) : null;
            return respons;
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