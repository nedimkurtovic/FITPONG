using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.Views.Users;
using FIT_PONG.SharedModels.Requests.Account;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Users
{
    class UnesiMejlViewModel : BaseViewModel
    {
        private readonly UsersAPIService usersService = new UsersAPIService();

        public UnesiMejlViewModel(string tip)
        {
            PotvrdiKomanda = new Command(() => PosaljiKonfirmacijskiMejl());
            Tip = tip;
        }

        private string _email;
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        private string Tip { get; set; }
        public ICommand PotvrdiKomanda { get; set; }


        public async void PosaljiKonfirmacijskiMejl()
        {
            if (isModelValid())
            {
                var obj = new Email_Password_Request
                {
                    Email = this.Email
                };

                if (Tip == "resetMail")
                {
                    var rezultat = await usersService.PosaljiKonfirmacijskiMejl(obj);

                    if (rezultat == default(SharedModels.Users))
                        await Application.Current.MainPage.DisplayAlert("Greska", "Doslo je do greske prilikom slanja mejla.", "OK");
                    else
                        Application.Current.MainPage = new PotvrdiMejlPassword(rezultat.ID, "resetMail", null);
                }
                else if (Tip == "resetPassword")
                {
                    var rezultat = await usersService.PosaljiMailZaPassword(obj);

                    if (rezultat == default(SharedModels.Users))
                        await Application.Current.MainPage.DisplayAlert("Greska", "Doslo je do greske prilikom slanja mejla.", "OK");
                    else
                        Application.Current.MainPage = new PotvrdiMejlPassword(rezultat.ID, "resetPassword", Email);
                }
            }
        }

        private bool isModelValid()
        {
            var listaErrora = new List<string>();

            //email
            if (String.IsNullOrEmpty(Email))
                listaErrora.Add("Email je obavezan.");
            else
            {
                var pattern = "[a-zA-Z0-9.]+@edu\\.fit\\.ba";
                Match match = Regex.Match(Email, pattern);
                if (!match.Success)
                    listaErrora.Add("Email mora biti u formatu ime.prezime@edu.fit.ba");
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
