using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.Views.Users;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Account;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Users
{
    class UsersEditViewModel: BaseViewModel
    {
        private readonly UsersAPIService usersApiService = new UsersAPIService();
        public UsersEditViewModel(SharedModels.Users user)
        {
            NapuniComboListe();
            this.User = user;
            PrikaznoIme = user.PrikaznoIme;
            Visina = user.Visina;
            JacaRuka = user.JacaRuka;
            //GradId = Gradovi.Where(d => d.Naziv == user.Grad).Select(d => d.ID).SingleOrDefault();
            //SelectedGrad = Gradovi.Where(d => d.ID == GradId).SingleOrDefault();
            SlikaProfila = user.ProfileImage;
        }

        

        public ObservableCollection<string> JacaRukaLista { get; set; } = new ObservableCollection<string>();
        public ObservableCollection<Gradovi> Gradovi { get; set; } = new ObservableCollection<Gradovi>();

        public SharedModels.Users User { get; set; }
        private string _prikaznoIme;
        public string PrikaznoIme { get => _prikaznoIme; set => SetProperty(ref _prikaznoIme, value); }
        private double? _visina;
        public double? Visina { get => _visina; set => SetProperty(ref _visina, value); }
        private string _jacaRuka;
        public string JacaRuka { get => _jacaRuka; set => SetProperty(ref _jacaRuka, value); }
        private int? _gradId;
        public int? GradId { get => _gradId; set => SetProperty(ref _gradId, value); }
        private Fajl _slikaProfila;
        public Fajl SlikaProfila { get => _slikaProfila; set => SetProperty(ref _slikaProfila, value); }
        private Gradovi _selectedGrad;
        public Gradovi SelectedGrad { get => _selectedGrad; set => SetProperty(ref _selectedGrad, value); }



        public async Task<SharedModels.Users> SpasiIzmjene()
        {
            if (isModelValid())
            {
                var obj = new AccountUpdate
                {
                    GradId = this.GradId,
                    JacaRuka = this.JacaRuka,
                    PrikaznoIme = this.PrikaznoIme,
                    Visina = this.Visina
                };

                var result = await usersApiService.Update<SharedModels.Users>(this.User.ID, obj, "PUT");

                return result;
            }

            return default(SharedModels.Users);
        }

        public async Task<SharedModels.Users> EditujSliku()
        {
            var obj = new SlikaPromjenaRequest
            {
                Naziv = this.SlikaProfila.Naziv,
                Slika = this.SlikaProfila.BinarniZapis
            };

            var rezultat = await usersApiService.UpdateProfilePicture(this.User.ID, obj);

            return rezultat;
        }

        public async Task<SharedModels.Users> ResetSliku()
        {
            var rezultat = await usersApiService.ResetProfilePicture(this.User.ID);

            return rezultat;
        }

        private bool isModelValid()
        {
            var listaErrora = new List<string>();

            //prikaznoIme
            if (String.IsNullOrEmpty(PrikaznoIme))
                listaErrora.Add("Prikazno ime je obavezno.");
            else
            {
                if (PrikaznoIme.Length > 50)
                    listaErrora.Add("Prikazno ime ne smije biti duze od 50 karaktera.");
                if (PrikaznoIme.Contains("@") || PrikaznoIme.Contains(" "))
                    listaErrora.Add("Prikazno ime ne smije sadrzavati @ i ' '.");
            }

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

            JacaRukaLista.Add("desna");
            JacaRukaLista.Add("lijeva");

            var gradoviService = new BaseAPIService("gradovi");
            var gradovi = await gradoviService.GetAll<List<Gradovi>>();

            foreach (var grad in gradovi)
                Gradovi.Add(grad);

            GradId = Gradovi.Where(d => d.Naziv == User.Grad).Select(d => d.ID).SingleOrDefault();
            SelectedGrad = Gradovi.Where(d => d.ID == GradId).SingleOrDefault();

        }
    }
}
