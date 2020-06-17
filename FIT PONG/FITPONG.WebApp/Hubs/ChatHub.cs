using FIT_PONG.Services.BL;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Hubs
{
    public class ChatHub:Hub
    {
        private readonly SignInManager<IdentityUser<int>> SignIn;
        private readonly FIT_PONG.Services.BL.Evidentor _evidentor;
        private static List<(string username, string connectionid)> ListaKonekcija = new List<(string username,string connectionid)>();

        public ChatHub(SignInManager<IdentityUser<int>> menadjer, FIT_PONG.Services.BL.Evidentor evidentor)
        {
            SignIn = menadjer;
            _evidentor = evidentor;
        }
        public override Task OnConnectedAsync()
        {
            string username = "anonim";
            if (SignIn.IsSignedIn(Context.User))
                username = _evidentor.NadjiIgraca(Context.User.Identity.Name).PrikaznoIme;
            string connid = Context.ConnectionId;
            ListaKonekcija.Add((username,connid));
            
            //Clients.Caller.SendAsync("GetKonekcije", GetAktivneKonekcije());
            Clients.All.SendAsync("GetKonekcije", GetAktivneKonekcije());

            return base.OnConnectedAsync();
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
            string posiljateljevoIme = _evidentor.NadjiIgraca(Context.User.Identity.Name).PrikaznoIme;
            string posiljatelj = ListaKonekcija.Where(x => x.username == posiljateljevoIme).FirstOrDefault().username;
            string vrijeme = DateTime.UtcNow.AddHours(1).ToString("hh:mm:ss");
            if (Primatelj != "Main")
            {
                string primatelj = ListaKonekcija.Where(x => x.username == Primatelj).FirstOrDefault().connectionid;
                //await Clients.Client().SendAsync("PrimiPoruku",poruka,posiljatelj, posiljatelj);
                await Clients.Clients(primatelj).SendAsync("PrimiPoruku", poruka, posiljatelj, posiljatelj,vrijeme);
                await Clients.Caller.SendAsync("PrimiPoruku", poruka, posiljatelj, Primatelj, vrijeme);
            }
            else
            {
                await Clients.All.SendAsync("PrimiPoruku", poruka, posiljatelj, "Main", vrijeme);
            }
        }
    }
    
}
