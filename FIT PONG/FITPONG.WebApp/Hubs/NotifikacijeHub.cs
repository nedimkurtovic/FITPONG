using FIT_PONG.Services;
using FIT_PONG.Services.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
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
#pragma warning disable IDE0059 // Unnecessary assignment of a value
            string username = "anonim";
#pragma warning restore IDE0059

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

            string connid = Context.ConnectionId;
            ListaKonekcija.Add((username, connid));
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

        public Task PosaljiNotifikacije(List<String> favoriti, string tim1, string tim2, int id)
        {
            List<String> aktivneKonekcije = GetAktivneKonekcije();

            var userId = db.Users.Where(d => d.Email == Context.User.Identity.Name).FirstOrDefault().Id;

            if (aktivneKonekcije.Contains(Context.User.Identity.Name) && favoriti.Contains(Context.User.Identity.Name))
                return Clients.User(userId.ToString()).SendAsync("PrimiNotifikacije", tim1, tim2, id);

            return null;
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

    }
}
