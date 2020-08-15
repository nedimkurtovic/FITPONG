using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.Views.Users;
using FIT_PONG.SharedModels.Requests.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Users
{
    class PotvrdiMejlPasswordViewModel: BaseViewModel
    {
        private readonly UsersAPIService usersService = new UsersAPIService();

        public PotvrdiMejlPasswordViewModel(int userId, string tip, string email)
        {
            PotvrdiKomanda = new Command(() => PotvrdiMejlPoziv());
            UserId = userId;
            isPasswordReset = tip == "resetPassword";
            Email = email;
        }

        private string _token;
        public string Token { get => _token; set => SetProperty(ref _token, value); }
        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        private string _potvrdaPassworda;
        public string PotvrdaPassworda { get => _potvrdaPassworda; set => SetProperty(ref _potvrdaPassworda, value); }
        public int UserId { get; set; }
        private string Email { get; set; }
        public bool isPasswordReset { get; set; }

        public ICommand PotvrdiKomanda { get; set; }
        

        public async void PotvrdiMejlPoziv()
        {
            if (isModelValid())
            {
                if (isPasswordReset)
                {
                    var obj = new PasswordPromjena
                    {
                        token = this.Token,
                        password = this.Password,
                        potvrdaPassword = this.PotvrdaPassworda,
                        Email = this.Email
                    };

                    var rezultat = await usersService.PotvrdiPassword(obj);

                    if (rezultat == default(SharedModels.Users))
                        await Application.Current.MainPage.DisplayAlert("Greska", "Doslo je do greske prilikom potvrde passworda.", "OK");
                    else
                        Application.Current.MainPage = new Views.Users.Login();
                }
                else
                {
                    var rezultat = await usersService.PotvrdiMejl(UserId, Token);

                    if (rezultat == default(SharedModels.Users))
                        await Application.Current.MainPage.DisplayAlert("Greska", "Doslo je do greske prilikom potvrde mejla.", "OK");
                    else
                        Application.Current.MainPage = new Views.Users.Login();
                }
            }
        }

        private bool isModelValid()
        {
            var listaErrora = new List<string>();

            if (String.IsNullOrEmpty(Token))
                listaErrora.Add("Token je obavezno polje.");

            if (isPasswordReset)
            {
                if (String.IsNullOrEmpty(Password))
                    listaErrora.Add("Password je obavezno polje.");

                if (String.IsNullOrEmpty(PotvrdaPassworda))
                    listaErrora.Add("Potvrda passworda je obavezno polje.");

                if (Password != PotvrdaPassworda)
                    listaErrora.Add("Passwordi se ne slazu.");
            }

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
