using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.Views.Users;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Users
{

    class RegistracijaViewModel : BaseViewModel
    {
        private readonly UsersAPIService usersService = new UsersAPIService();

        public RegistracijaViewModel()
        {
            NapuniComboListe();
            RegistracijaKomanda = new Command(() => Registracija());
        }


        public ObservableCollection<string> Spolovi { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<string> JacaRukaLista { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Gradovi> Gradovi { get; set; } = new ObservableCollection<Gradovi>();

        private string _email;
        public string Email { get => _email; set => SetProperty(ref _email, value); }
        private string _prikaznoIme;
        public string PrikaznoIme { get => _prikaznoIme; set => SetProperty(ref _prikaznoIme, value); }
        private string _password;
        public string Password { get => _password; set => SetProperty(ref _password, value); }
        private string _potvrdaPassworda;
        public string PotvrdaPassworda { get => _potvrdaPassworda; set => SetProperty(ref _potvrdaPassworda, value); }
        private double? _visina;
        public double? Visina { get => _visina; set => SetProperty(ref _visina, value); }
        private string _jacaRuka;
        public string JacaRuka { get => _jacaRuka; set => SetProperty(ref _jacaRuka, value); }
        private char _spol;
        public char Spol { get => _spol; set => SetProperty(ref _spol, value); }
        private int? _gradId;
        public int? GradId { get => _gradId; set => SetProperty(ref _gradId, value); }
        private Fajl _slikaProfila;
        public Fajl SlikaProfila { get => _slikaProfila; set => SetProperty(ref _slikaProfila, value); }
        public ICommand RegistracijaKomanda { get; set; }


        public async void Registracija()
        {
            if (isModelValid()) {

                AccountInsert obj = new AccountInsert
                {
                    Email = this.Email,
                    PrikaznoIme = this.PrikaznoIme,
                    Password = this.Password,
                    PotvrdaPassword = this.PotvrdaPassworda,
                    Visina = this.Visina,
                    JacaRuka = this.JacaRuka,
                    Spol = this.Spol,
                    GradId = this.GradId,
                    ELO = 1000,
                    BrojPosjetaNaProfil = 0,
                    Slika = SlikaProfila
                };

                var result = await usersService.Registracija(obj);

                if (result != default(SharedModels.Users))
                {
                    Application.Current.MainPage = new PotvrdiMejlPassword(result.ID, "resetMail", null);
                    return;
                }
                await Application.Current.MainPage.DisplayAlert("Greska", "Greska prilikom registracije.", "OK"); 
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

            //prikaznoIme
            if (String.IsNullOrEmpty(PrikaznoIme))
                listaErrora.Add("Prikazno ime je obavezno.");
            else{
                if (PrikaznoIme.Length > 50)
                    listaErrora.Add("Prikazno ime ne smije biti duze od 50 karaktera.");
                if (PrikaznoIme.Contains("@") || PrikaznoIme.Contains(" "))
                    listaErrora.Add("Prikazno ime ne smije sadrzavati @ i ' '.");
            }

            //password / potvrdaPassworda
            if (String.IsNullOrEmpty(Password) || String.IsNullOrEmpty(PotvrdaPassworda))
                listaErrora.Add("Password i potvrda passworda su obavezna polja.");
            if (Password != PotvrdaPassworda)
                listaErrora.Add("Passwordi se ne slazu.");

            //visina
            if (Visina < 1 || Visina > 300)
                listaErrora.Add("Visina treba biti u rasponu 1-300.");


            if (listaErrora.Count == 0)
                return true;
         
            StringBuilder builder = new StringBuilder();
            foreach (var i in listaErrora)
                builder.AppendLine(i);
            Application.Current.MainPage.DisplayAlert("Greška", builder.ToString(), "OK");
            return false;
        }


        private async void NapuniComboListe()
        {
            Spolovi.Add("M");
            Spolovi.Add("Ž");

            JacaRukaLista.Add("desna");
            JacaRukaLista.Add("lijeva");

            var gradoviService = new BaseAPIService("gradovi");
            var gradovi = await gradoviService.GetAll<List<Gradovi>>();

            foreach (var grad in gradovi)
                Gradovi.Add(grad);
        }

    }
}
