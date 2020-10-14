using FIT_PONG.Services;
using FIT_PONG.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Org.BouncyCastle.Math.EC.Rfc7748;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FIT_PONG.Hubs
{
    public class NotifikacijeHub : Hub
    {
        private readonly IUsersService _userService;
        private readonly SignInManager<IdentityUser<int>> signIn;
        private readonly FIT_PONG.Database.MyDb db;

        public static List<(string username, string connectionid)> ListaKonekcija = new List<(string username, string connectionid)>();


        public NotifikacijeHub(SignInManager<IdentityUser<int>> signIn, FIT_PONG.Database.MyDb db, IUsersService usersService)
        {
            this.signIn = signIn;
            this.db = db;
            this._userService = usersService;
        }

        public async override Task OnConnectedAsync()
        {
            string username = "anonim";

            if (Context.User.Identity.Name == null)
            {
                var rezultatAuth = await ProvjeriAuth(Context.GetHttpContext().Request);
                if (rezultatAuth == null)
                {
                    Context.Abort();
                    return;
                }
                else
                    username = rezultatAuth;
            }
            else
                username = Context.User.Identity.Name;

            if(username != "anonim")
                PromijeniBrojKorisnika(1);
            string connid = Context.ConnectionId;
            ListaKonekcija.Add((username, connid));
            await Clients.All.SendAsync("GetKonekcije", GetAktivneKonekcije());
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            (string username, string connid) nadjeni = ListaKonekcija
                .Where(x => x.connectionid == Context.ConnectionId).FirstOrDefault();
            
            if(nadjeni != (null,null))
                PromijeniBrojKorisnika(-1);

            ListaKonekcija.Remove(nadjeni);

            Clients.All.SendAsync("GetKonekcije", GetAktivneKonekcije());
            return base.OnDisconnectedAsync(exception);
        }

        public List<string> GetAktivneKonekcije()
        {
            return ListaKonekcija.Select(x => x.username).ToList();
        }

        public async Task PosaljiNotifikacije(List<String> favoriti, string tim1, string tim2, int id)
        {
            List<String> aktivneKonekcije = GetAktivneKonekcije();
            string username = "anonim";


            if (Context.User.Identity.Name == null)
            {
                var rezultatAuth = await ProvjeriAuth(Context.GetHttpContext().Request);
                if (rezultatAuth == null)
                {
                    Context.Abort();
                    return;
                }
                else
                    username = rezultatAuth;
            }
            else
                username = Context.User.Identity.Name;

            var userId = db.Users.Where(d => d.Email == username).FirstOrDefault().Id;
            var connectionid = ListaKonekcija.Where(x => x.username == username).FirstOrDefault().connectionid;
            if (aktivneKonekcije.Contains(username) && favoriti.Contains(username)) {
                //await Clients.User(userId.ToString()).SendAsync("PrimiNotifikacije", tim1, tim2, id);
                await Clients.Client(connectionid).SendAsync("PrimiNotifikacije", tim1, tim2, id);
            }
            return;
        }
        public async Task DummyStartaj(List<string> favoriti, string tim1, string tim2, int id)
        {
            await Clients.All.SendAsync("startaj", favoriti, tim1, tim2, id);
        }

        public async Task<string> ProvjeriAuth(HttpRequest Request)
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return null;
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            FIT_PONG.SharedModels.Users user = null;
#pragma warning restore IDE0059 // Unnecessary assignment of a value
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':');
                var username = credentials[0];
                var password = credentials[1];
                user = await _userService.CheckValidanLogin(
                new FIT_PONG.SharedModels.Requests.Account.Login
                {
                    UserName = username,
                    Password = password
                });

            }
            catch (Exception)
            {
                return null;
            }

            if (user == null)
                return null;
            return user.PrikaznoIme;
        }
        private void PromijeniBrojKorisnika(int operacija)
        {
            var KorisnikLog = db.BrojKorisnikaLog.Where(x => x.Datum.Date == DateTime.Now.Date).FirstOrDefault();
            if (KorisnikLog == null)
            {
                PromijeniPrethodnu(); // ako je bila promjena datuma onda ce ostati broj aktivnih korisnika
                KorisnikLog = new Database.DTOs.BrojKorisnikaLog
                {
                    MaxBrojKorisnika = ListaKonekcija.Count(),
                    Datum = DateTime.Now.Date
                };
                db.BrojKorisnikaLog.Add(KorisnikLog);    
            }
            KorisnikLog.BrojKorisnika = ListaKonekcija.Count(); // u slucaju pada servera, da ne bude da je nastavio od zadnje vrijednosti
            if (KorisnikLog.BrojKorisnika == 0 && operacija == -1)//za svaki slucaj ovaj uslov
                return;
            KorisnikLog.BrojKorisnika += operacija;
            if (KorisnikLog.BrojKorisnika > KorisnikLog.MaxBrojKorisnika)
                KorisnikLog.MaxBrojKorisnika = KorisnikLog.BrojKorisnika;
            db.SaveChanges();
        }
        private void PromijeniPrethodnu()
        {
            var KorisnikLog = db.BrojKorisnikaLog.OrderByDescending(x => x.ID).FirstOrDefault();
            if (KorisnikLog != null)
                KorisnikLog.BrojKorisnika = 0;
            db.SaveChanges();
        }
    }
}
