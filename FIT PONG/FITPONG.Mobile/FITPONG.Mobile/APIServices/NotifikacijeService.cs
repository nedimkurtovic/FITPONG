using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.APIServices
{
    public class NotifikacijeService
    {
#if DEBUG
        protected string hubUrl = "http://localhost:4260/notifikacije";
#endif
#if RELEASE
        protected string hubUrl = "http://p1869.app.fit.ba/notifikacije";
#endif

        public event EventHandler<MessageEventArgs> primiNotifikacije;
        public event EventHandler<MessageEventArgs> OnConnectionClosed;


        public ObservableCollection<string> ListaKonekcija { get; set; }
        public bool IsConnected { get; set; }

        HubConnection hubKonekcija;
        Random random;

        public void Init()
        {
            IsConnected = false;
            random = new Random();
            ListaKonekcija = new ObservableCollection<string>();

            string username = BaseAPIService.Username;
            string password = BaseAPIService.Password;
            string kredencijali = Convert.ToBase64String(ASCIIEncoding.ASCII.GetBytes(username + ":" + password));

            hubKonekcija = new HubConnectionBuilder()
                .WithUrl(hubUrl, opcije => opcije.Headers.Add("Authorization", "Basic " + kredencijali))
                .Build();

            hubKonekcija.Closed += async (error) =>
            {
                OnConnectionClosed?.Invoke(this, new MessageEventArgs("Disconnected.."));
                IsConnected = false;
                await Task.Delay(random.Next(0, 5) * 1000);
                try
                {
                    await ConnectAsync();
                }
                catch (Exception)
                {
                    // Exception!
                }
            };

            hubKonekcija.On<List<string>>("GetKonekcije", (_listaKonekcija) =>
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    ListaKonekcija.Clear();
                    foreach (var i in _listaKonekcija)
                        ListaKonekcija.Add(i);
                });
            });

            hubKonekcija.On<string, string, int>("PrimiNotifikacije", (tim1, tim2, id) =>
            {
                var notifikacija = "Evidentirana je utakmica izmedju " + tim1 + " i " + tim2;
                primiNotifikacije?.Invoke(this, new MessageEventArgs(notifikacija));
            });
            hubKonekcija.On<List<string>,string, string, int>("startaj", (List<string> favoriti, string tim1, string tim2, int id) =>
            {
                _ = PosaljiNotifikacijeAsync(favoriti, tim1, tim2, id);
            });
        }


        public async Task ConnectAsync()
        {
            if (IsConnected)
                return;

            await hubKonekcija.StartAsync();
            IsConnected = true;

        }

        public async Task DisconnectAsync()
        {
            if (!IsConnected)
                return;
            await hubKonekcija.StopAsync();
            IsConnected = false;

        }

        public async Task PosaljiNotifikacijeAsync(List<string> favoriti, string tim1, string tim2, int id)
        {
            if (!IsConnected)
                return;
            var userName = BaseAPIService.Username;
            await hubKonekcija.InvokeAsync("PosaljiNotifikacije", favoriti, tim1, tim2, id);
        }
        public async Task DummyStartajAsync(List<string> favoriti, string tim1, string tim2, int id)
        {
            if (!IsConnected)
                return;
            var userName = BaseAPIService.Username;
            await hubKonekcija.InvokeAsync("DummyStartaj", favoriti, tim1, tim2, id);
        }
    }
}
