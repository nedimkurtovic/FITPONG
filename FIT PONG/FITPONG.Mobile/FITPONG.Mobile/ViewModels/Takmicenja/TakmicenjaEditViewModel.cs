using FIT_PONG.Mobile.APIServices;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja
{
    public class TakmicenjaEditViewModel:BaseViewModel
    {
        private SharedModels.Takmicenja Takmicenje { get; set; }
        public TakmicenjaEditViewModel(SharedModels.Takmicenja _takmicenje)
        {
            RokPocetkaPrijave = _takmicenje.DatumPocetkaPrijava;
            RokZavrsetkaPrijave = _takmicenje.DatumZavrsetkaPrijava;
            DatumPocetka = _takmicenje.DatumPocetka;
            MinimalniELO = _takmicenje.MinimalniELO;
            Naziv = _takmicenje.Naziv;
            Takmicenje = _takmicenje;
        }
        public TakmicenjeAPIService takmicenjeAPIService { get; set; } = new TakmicenjeAPIService();
        private DateTime? _rokpocetkaprijave;
        public DateTime? RokPocetkaPrijave { get => _rokpocetkaprijave; set => SetProperty(ref _rokpocetkaprijave, value); }

        private DateTime? _rokzavrsetkaprijave;
        public DateTime? RokZavrsetkaPrijave { get => _rokzavrsetkaprijave; set => SetProperty(ref _rokzavrsetkaprijave, value); }

        private DateTime? _datumpocetka;
        public DateTime? DatumPocetka { get => _datumpocetka; set => SetProperty(ref _datumpocetka, value); }

        private int? _minimalniELO;
        public int? MinimalniELO { get => _minimalniELO; set => SetProperty(ref _minimalniELO, value); }

        private string _naziv;
        public string Naziv { get => _naziv; set => SetProperty(ref _naziv, value); }

        public bool DatumiPrijavaVisible() { return !Takmicenje.Inicirano ?? true; }
        public async Task<SharedModels.Takmicenja> PozoviApi()
        {
            if (!Validacija()) 
                return default(SharedModels.Takmicenja);

            var obj = new SharedModels.Requests.Takmicenja.TakmicenjaUpdate
            {
                DatumPocetka = this.DatumPocetka,
                MinimalniELO = this.MinimalniELO,
                Naziv = this.Naziv,
                RokPocetkaPrijave = this.RokPocetkaPrijave,
                RokZavrsetkaPrijave = this.RokZavrsetkaPrijave
            };
            var rezultat = await takmicenjeAPIService.Update<SharedModels.Takmicenja>(Takmicenje.ID, obj,"PATCH");
            return rezultat;
        }
        public bool Validacija()
        {
            var listaErrora = new List<string>();

            if (Naziv.Length > 100)
                listaErrora.Add("Naziv ne može sadržavati više od 100 karaktera.");

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
