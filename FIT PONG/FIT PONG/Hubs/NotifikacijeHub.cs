using FIT_PONG.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FIT_PONG.Hubs
{
    public class NotifikacijeHub : Hub
    {
        private readonly SignInManager<IdentityUser<int>> signIn;
        private readonly MyDb db;

        public static List<(string username, string connectionid)> ListaKonekcija = new List<(string username, string connectionid)>();


        public NotifikacijeHub(SignInManager<IdentityUser<int>> signIn, MyDb db)
        {
            this.signIn = signIn;
            this.db = db;
        }

        public override Task OnConnectedAsync()
        {
            string username = Context.User.Identity.Name;
            string connid = Context.ConnectionId;
            ListaKonekcija.Add((username, connid));

            return base.OnConnectedAsync();
        }
        public override Task OnDisconnectedAsync(Exception exception)
        {
            (string username, string connid) nadjeni = ListaKonekcija
                .Where(x => x.connectionid == Context.ConnectionId).FirstOrDefault();
            ListaKonekcija.Remove(nadjeni);

            return base.OnDisconnectedAsync(exception);
        }

        public List<string> GetAktivneKonekcije()
        {
            return ListaKonekcija.Select(x => x.username).ToList();
        }

        public Task PosaljiNotifikacije(List<String> favoriti, string tim1, string tim2, int id)
        {
            List<String> aktivneKonekcije = GetAktivneKonekcije();

            var userId = db.Users.Where(d => d.Email == Context.User.Identity.Name).FirstOrDefault().Id;

            if (aktivneKonekcije.Contains(Context.User.Identity.Name) && favoriti.Contains(Context.User.Identity.Name))
                return Clients.User(userId.ToString()).SendAsync("PrimiNotifikacije", tim1, tim2, id);

            return null;
        }

    }
}
