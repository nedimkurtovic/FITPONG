using FIT_PONG.Mobile.APIServices;
using FIT_PONG.SharedModels;
using FIT_PONG.WebAPI.Services.Bazni;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Reports
{
    public class ReportsDodajViewModel: BaseViewModel
    {
        public ReportsDodajViewModel()
        {
            reportsAPIService = new BaseAPIService("reports");
            Prilozi = new ObservableCollection<Fajl>();
        }

        public BaseAPIService reportsAPIService { get; set; }

        private string _naslov;
        public string Naslov { get => _naslov; set=>SetProperty(ref _naslov,value); }

        private string _sadrzaj;
        public string Sadrzaj { get =>_sadrzaj; set => SetProperty(ref _sadrzaj, value); }

        private string _email;
        public string Email { get=>_email; set => SetProperty(ref _email, value); }

        public ObservableCollection<Fajl> Prilozi { get; set; }

        public async Task<SharedModels.Reports> PosaljiReport()
        {
            IsBusy = true;
            if (!Validacija())
                return default(SharedModels.Reports);
            //mislim da ce moc da stavim BaseAPIService.Username kao email
            var userName = BaseAPIService.Username;
            //iz nekog cudnog razloga kad pozoves ovaj BaseAPIService.Username unutar ovog kreiranja objekta ne vidi ovaj username?
            var noviRequest = new SharedModels.Requests.Reports.ReportsInsert
            {
                DatumKreiranja = DateTime.Now,
                Email = userName,
                Naslov = this.Naslov,
                Sadrzaj = this.Sadrzaj,
                Prilozi = new List<Fajl>()
            };
            foreach (var i in Prilozi)
                noviRequest.Prilozi.Add(i);
            //vjerovatno moze i prilozi = this.prilozi.tolist();

            var rezultat = await reportsAPIService.Insert<SharedModels.Reports>(noviRequest);
            return rezultat;
        }
        private bool Validacija()
        {
            var listaErrora = new List<string>();
            if (String.IsNullOrEmpty(Naslov) || String.IsNullOrWhiteSpace(Naslov))
                listaErrora.Add("Morate unijeti naslov");
            if (String.IsNullOrEmpty(Sadrzaj) || String.IsNullOrWhiteSpace(Sadrzaj))
                listaErrora.Add("Morate unijeti sadržaj");


            if (listaErrora.Count == 0)
                return true;
            StringBuilder novi = new StringBuilder();
            foreach (var i in listaErrora)
                novi.AppendLine(i);
            Application.Current.MainPage.DisplayAlert("Greška", novi.ToString(), "OK");
            return false;
        }
    }
}
