using FIT_PONG.Mobile.ViewModels.Chat;
using Microsoft.AspNetCore.SignalR.Client;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.APIServices
{
    public class MessageEventArgs
    {
        public MessageEventArgs(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }

    public class ChatService
    {
#if DEBUG
        //protected string hubUrl = "http://localhost:5766/ChatHub";
        protected string hubUrl = "http://localhost:4260/chathub";
#endif
#if RELEASE
        protected string hubUrl = "http://localhost:4260/chathub";
#endif

        public event EventHandler<FIT_PONG.Mobile.ViewModels.Chat.ChatPoruka> StiglaPoruka;
        public event EventHandler<MessageEventArgs> OnConnectionClosed;

        public ObservableCollection<string> ListaKonekcija { get; set; }

        HubConnection hubKonekcija;
        Random random;

        public bool IsConnected { get; set; }
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

            hubKonekcija.On<string, string, string,string>("PrimiPoruku", (poruka, posiljatelj, primatelj, vrijeme) =>
            {
                ChatPoruka nova = new ChatPoruka(poruka, posiljatelj, primatelj, vrijeme);
                StiglaPoruka?.Invoke(this, nova);
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


        public async Task PosaljiPorukuAsync(string poruka, string primatelj)
        {
            await hubKonekcija.InvokeAsync("PosaljiPoruku",poruka,primatelj);
        }
    }
}
