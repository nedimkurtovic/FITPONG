using FIT_PONG.Mobile.APIServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Chat
{
    public class ChatKonverzacijaViewModel:BaseViewModel
    {
        public ChatKonverzacijaViewModel(ChatService _chatservis, string _GrupaPrimatelj)
        {
            Title = _GrupaPrimatelj;

            ChatServis = _chatservis;
            Poruke = new ObservableCollection<ChatPoruka>();
            random = new Random();
            GrupaPrimatelj = _GrupaPrimatelj; // za sada se odnosi samo na Main ili samo na jedan username

            PosaljiPorukuCommand = new Command(async () => await PosaljiPoruku());
            ConnectCommand = new Command(async () => await Connect());
            DisconnectCommand = new Command(async () => await Disconnect());


        }
        public string GrupaPrimatelj { get; set; }
        public ChatService ChatServis { get; set; }

        bool isConnected;
        public bool IsConnected
        {
            get => ChatServis.IsConnected;
            set
            {
                Device.BeginInvokeOnMainThread(() =>
                {
                    SetProperty(ref isConnected, value);
                });
            }
        }
        public ObservableCollection<ChatPoruka> Poruke { get; set; }
        Random random;

        string _porukaTextBox;
        public string PorukaTextbox { get => _porukaTextBox; set => SetProperty(ref _porukaTextBox, value); }

        public Command PosaljiPorukuCommand { get; set; }
        public Command ConnectCommand { get; set; }
        public Command DisconnectCommand { get; set; }

        async Task Connect()
        {
            if (IsConnected)
                return;
            try
            {
                await ChatServis.ConnectAsync();
                IsConnected = true;
                ChatPoruka nova = new ChatPoruka("Connected..", "", "", DateTime.UtcNow.AddHours(1).ToString("hh:mm:ss"));
                SendLocalMessage(nova);
            }
            catch (Exception ex)
            {
                ChatPoruka novaErr = new ChatPoruka($"Greška pri konekciji: {ex.Message}",
                    "", "", DateTime.UtcNow.AddHours(1).ToString("hh:mm:ss"));
                SendLocalMessage(novaErr);
            }
        }

        async Task Disconnect()
        {
            if (!IsConnected)
                return;
            await ChatServis.DisconnectAsync();
            IsConnected = false;
            ChatPoruka nova = new ChatPoruka("Disconnected..", "", "", DateTime.UtcNow.AddHours(1).ToString("hh:mm:ss"));
            SendLocalMessage(nova);
        }

        async Task PosaljiPoruku()
        {
            if (!IsConnected)
            {
                await Application.Current.MainPage.DisplayAlert("Niste konektovani", "Pokušajte se konektovati pa onda poslati poruku.", "OK");
                return;
            }
            try
            {
                IsBusy = true;
                //await ChatService.SendMessageAsync(Settings.Group,
                //    Settings.UserName,
                //    ChatMessage.Message);
                await ChatServis.PosaljiPorukuAsync(PorukaTextbox, GrupaPrimatelj);

                PorukaTextbox = string.Empty;
            }
            catch (Exception ex)
            {
                ChatPoruka nova = new ChatPoruka($"Slanje neuspješno: {ex.Message}", "", "", DateTime.UtcNow.AddHours(1).ToString("hh:mm:ss"));
                SendLocalMessage(nova);
            }
            finally
            {
                IsBusy = false;
            }
        }
        public void SendLocalMessage(ChatPoruka poruka)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                Poruke.Add(poruka);
            });
        }
    }
    public class ChatPoruka
    {
        public string Posiljatelj { get; set; }
        public string Primatelj { get; set; }
        public string Sadrzaj { get; set; }
        public string Vrijeme { get; set; }
        public ChatPoruka(string poruka, string posiljatelj, string primatelj, string vrijeme)
        {
            this.Posiljatelj = posiljatelj;
            this.Primatelj = primatelj;
            this.Sadrzaj = poruka;
            this.Vrijeme = vrijeme;
        }
        public ChatPoruka()
        {

        }
        string nesto;
        public string MessageFinal { get 
            {
                return Vrijeme + " | " + Posiljatelj + " : " + Sadrzaj;
            } 
        }
    }
}
