using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.Views.Takmicenja;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja
{
    public class TakmicenjaDodajViewModel:BaseViewModel,INotifyPropertyChanged
    {
        public TakmicenjaDodajViewModel()
        {
            Seeded = false;
            RucniOdabir = false;
            Title = "Dodaj novo takmičenje";
            DodajNovoTakmicenje = new Command(async () => await DodajTakmicenjeFunkcija());
            ListaKategorija = new ObservableCollection<SharedModels.KategorijeTakmicenja>();
            ListaVrsta = new ObservableCollection<SharedModels.VrsteTakmicenja>();
            ListaSistema = new ObservableCollection<SharedModels.SistemiTakmicenja>();
            KategorijaID = -1;
            VrstaID = -1;
            SistemID = -1;
        }
        public TakmicenjeAPIService TakmicenjeAPIService { get; set; } = new TakmicenjeAPIService();
        public ObservableCollection<SharedModels.KategorijeTakmicenja> ListaKategorija { get; set; }
        public List<string> ListaKat { get; set; } = new List<string> { "necko","Pecko","stasad","RadiLi" };
        public ObservableCollection<SharedModels.VrsteTakmicenja> ListaVrsta { get; set; }
        public ObservableCollection<SharedModels.SistemiTakmicenja> ListaSistema { get; set; }

        private string _naziv;
        public string Naziv { get => _naziv; set => SetProperty(ref _naziv, value); }

        private DateTime? _rokpocetkaprijave;
        public DateTime? RokPocetkaPrijave { get=> _rokpocetkaprijave; set => SetProperty(ref _rokpocetkaprijave,value); }
        
        private DateTime? _rokzavrsetkaprijave;
        public DateTime? RokZavrsetkaPrijave { get=>_rokzavrsetkaprijave; set=>SetProperty(ref _rokzavrsetkaprijave,value); }

        private int? _minimalniELO;
        public int? MinimalniELO { get => _minimalniELO; set => SetProperty(ref _minimalniELO, value); }

        private bool _seeded;
        public bool Seeded { get=>_seeded; set => SetProperty(ref _seeded,value); }

        private int _kategorija;
        public int KategorijaID { get => _kategorija; set => SetProperty(ref _kategorija, value); }

        private int _sistem;
        public int SistemID { get=>_sistem; set=>SetProperty(ref _sistem,value); }

        private int _vrsta;
        public int VrstaID { get=>_vrsta; set => SetProperty(ref _vrsta,value); }

        private bool _rucni;
        public bool RucniOdabir { get => _rucni; set => SetProperty(ref _rucni, value); }

        private string _rucnoodabraniigraci;
        public string RucnoOdabraniIgraci { get => _rucnoodabraniigraci; set => SetProperty(ref _rucnoodabraniigraci,value); }

        private DateTime? _datumpocetka;
        public DateTime? DatumPocetka { get => _datumpocetka; set=>SetProperty(ref _datumpocetka,value); }
        
        private DateTime? _datumzavrsetka;
        public DateTime? DatumZavrsetka { get => _datumzavrsetka; set => SetProperty(ref _datumzavrsetka, value); }

        public ICommand DodajNovoTakmicenje { get; set; }
        private bool _vidljivost;
        public bool RucnaVidljivost
        {
            get { return _vidljivost; }
            set
            {
                _vidljivost = value;
                NotifyPropertyChanged(nameof(RucnaVidljivost));
            }
        }
        public bool VidljivostPrijava { get => !_vidljivost;}
        private void NotifyPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
#pragma warning disable CS0108 // Member hides inherited member; missing new keyword
        public event PropertyChangedEventHandler PropertyChanged;
#pragma warning restore CS0108 // Member hides inherited member; missing new keyword
        public async Task<SharedModels.Takmicenja> DodajTakmicenjeFunkcija()
        {
            if (!Validacija())
                return default(SharedModels.Takmicenja);
            TakmicenjaInsert obj = new TakmicenjaInsert
            {
                Naziv = this.Naziv,
                RokPocetkaPrijave = this.RokPocetkaPrijave,
                RokZavrsetkaPrijave = this.RokZavrsetkaPrijave,
                RucniOdabir = this.RucniOdabir, 
                RucnoOdabraniIgraci = this.RucnoOdabraniIgraci,
                DatumPocetka = this.DatumPocetka,
                DatumZavrsetka = this.DatumZavrsetka,
                MinimalniELO = this.MinimalniELO,
                Seeded = this.Seeded,
                KategorijaID = this.KategorijaID,
                SistemID = this.SistemID,
                VrstaID = this.VrstaID
            };
            var rezultat = await TakmicenjeAPIService.Insert<SharedModels.Takmicenja>(obj);
            return rezultat;

        }
        public async void NapuniComboBoxes()
        {
            var servis = new BaseAPIService("kategorije-takmicenja");
            var listak = await servis.GetAll<List<SharedModels.KategorijeTakmicenja>>(null);
            NapuniKolekciju<SharedModels.KategorijeTakmicenja>(ListaKategorija, listak);

            servis = new BaseAPIService("vrste-takmicenja");
            var listav = await servis.GetAll<List<SharedModels.VrsteTakmicenja>>(null);
            NapuniKolekciju<SharedModels.VrsteTakmicenja>(ListaVrsta, listav);

            servis = new BaseAPIService("sistemi-takmicenja");
            var listas = await servis.GetAll<List<SharedModels.SistemiTakmicenja>>(null);
            NapuniKolekciju<SharedModels.SistemiTakmicenja>(ListaSistema, listas);

        }
        public bool Validacija()
        {
            var listaErrora = new List<string>();
            if (String.IsNullOrEmpty(Naziv) || String.IsNullOrWhiteSpace(Naziv))
                listaErrora.Add("Morate unijeti naziv");
            if(Naziv.Length > 100)
                listaErrora.Add("Naziv ne može sadržavati više od 100 karaktera");
            if (!ListaKategorija.Select(x => x.ID).Contains(KategorijaID))
                listaErrora.Add("Morate odabrati kategoriju");
            if (!ListaVrsta.Select(x => x.ID).Contains(VrstaID))
                listaErrora.Add("Morate odabrati vrstu");
            if (!ListaSistema.Select(x => x.ID).Contains(SistemID))
                listaErrora.Add("Morate odabrati sistem");

            if (listaErrora.Count == 0)
                return true;
            StringBuilder novi = new StringBuilder();
            foreach (var i in listaErrora)
                novi.AppendLine(i);
            Application.Current.MainPage.DisplayAlert("Greška",novi.ToString(),"OK");
            return false;
        }
        void NapuniKolekciju<T>(ObservableCollection<T> kol, List<T> kolekcija2)
        {
            foreach (var i in kolekcija2)
                kol.Add(i);
        }
    }
}
