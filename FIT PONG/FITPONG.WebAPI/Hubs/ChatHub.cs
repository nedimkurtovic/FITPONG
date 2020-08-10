using FIT_PONG.Services.Services;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace FIT_PONG.WebAPI.Hubs
{
    [EnableCors("CorsPolicy")]
    public class ChatHub:Hub
    {
        private readonly IUsersService _userService;
        private readonly FIT_PONG.Services.BL.Evidentor _evidentor;
        //treba mi username
        private static List<(string username, string connectionid)> ListaKonekcija = new List<(string username, string connectionid)>();

        public ChatHub(IUsersService _menadjer, FIT_PONG.Services.BL.Evidentor evidentor)
        {
            _userService = _menadjer;
            _evidentor = evidentor;
        }
        [EnableCors("CorsPolicy")]
        public async override Task OnConnectedAsync()
        {
            string username = "anonim";
            //if (SignIn.IsSignedIn(Context.User))
            //    username = _evidentor.NadjiIgraca(Context.User.Identity.Name).PrikaznoIme;

            //moram skontat kako logiku napravit takvu da prvo provjerim da li je web api user, tj da li je slao request header,
            //ako jest onda postavi username na to sto vrati i to je to , ako nije dobar provjeri da li je "web user" kako ces to znat?
            //zato sto koristimo user identity tamo na web appu i logujemo sesije koje se spremaju u Context.User.Identity

            
            if(Context.User.Identity.Name == null)
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
                username = _evidentor.NadjiIgraca(Context.User.Identity.Name).PrikaznoIme;
            //ovo se u nasoj aplikaciji nikad ne bi trebalo desiti ako prodje login nego samo zastita za 
            //ove malo sto vole istrazivati 

            string connid = Context.ConnectionId;
            ListaKonekcija.Add((username, connid));
            
            //Clients.Caller.SendAsync("GetKonekcije", GetAktivneKonekcije());
            await Clients.All.SendAsync("GetKonekcije", GetAktivneKonekcije());
            await base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            (string username, string connid) nadjeni = ListaKonekcija
                .Where(x => x.connectionid == Context.ConnectionId).FirstOrDefault();
            ListaKonekcija.Remove(nadjeni);

            Clients.All.SendAsync("GetKonekcije", GetAktivneKonekcije());
            return base.OnDisconnectedAsync(exception);
        }
        public List<string> GetAktivneKonekcije()
        {
            return ListaKonekcija.Select(x => x.username).ToList();
        }
        public async Task PosaljiPoruku(string poruka, string Primatelj)
        {
            var posiljatelj = ListaKonekcija.Where(x => x.connectionid == Context.ConnectionId).FirstOrDefault().username;
            //string posiljateljevoIme = _evidentor.NadjiIgraca(Context.User.Identity.Name).PrikaznoIme;
            //string posiljatelj = ListaKonekcija.Where(x => x.username == posiljateljevoIme).FirstOrDefault().username;
            string vrijeme = DateTime.UtcNow.AddHours(1).ToString("hh:mm:ss");
            if (Primatelj != "Main")
            {
                string primatelj = ListaKonekcija.Where(x => x.username == Primatelj).FirstOrDefault().connectionid;
                //await Clients.Client().SendAsync("PrimiPoruku",poruka,posiljatelj, posiljatelj);
                await Clients.Clients(primatelj).SendAsync("PrimiPoruku", poruka, posiljatelj, posiljatelj, vrijeme);
                await Clients.Caller.SendAsync("PrimiPoruku", poruka, posiljatelj, Primatelj, vrijeme);
            }
            else
            {
                await Clients.All.SendAsync("PrimiPoruku", poruka, posiljatelj, "Main", vrijeme);
            }
        }

        public async Task<string> ProvjeriAuth(HttpRequest Request)
        {
            if (!Request.Headers.ContainsKey("Authorization"))
                return null;
            FIT_PONG.SharedModels.Users user = null;
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
            catch(Exception ex)
            {
                return null;
            }

            if (user == null)
                return null;
            return user.PrikaznoIme;
        }

    }
}
