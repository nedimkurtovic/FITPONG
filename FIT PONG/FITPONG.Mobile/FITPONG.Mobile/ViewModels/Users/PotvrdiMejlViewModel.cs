using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.Views.Users;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Users
{
    class PotvrdiMejlViewModel: BaseViewModel
    {
        private readonly UsersAPIService usersService = new UsersAPIService();

        public PotvrdiMejlViewModel(int userId)
        {
            PotvrdiKomanda = new Command(() => PotvrdiMejlPoziv());
            UserId = userId;
        }

        private string _token;
        public string Token { get => _token; set => SetProperty(ref _token, value); }
        public int UserId { get; set; }
        public ICommand PotvrdiKomanda { get; set; }
        

        public async void PotvrdiMejlPoziv()
        {
            if (isModelValid())
            {
                var rezultat = await usersService.PotvrdiMejl(UserId, Token);

                if (rezultat == default(SharedModels.Users))
                    await Application.Current.MainPage.DisplayAlert("Greska", "Doslo je do greske prilikom potvrde mejla.", "OK");

                Application.Current.MainPage = new Login();
            }
        }

        private bool isModelValid()
        {
            var listaErrora = new List<string>();

            if (String.IsNullOrEmpty(Token))
                listaErrora.Add("Token je obavezno polje.");

            if (listaErrora.Count == 0)
                return true;

            StringBuilder builder = new StringBuilder();
            foreach (var i in listaErrora)
                builder.AppendLine(i);
            Application.Current.MainPage.DisplayAlert("Greška", builder.ToString(), "OK");
            return false;
        }
    }
}
