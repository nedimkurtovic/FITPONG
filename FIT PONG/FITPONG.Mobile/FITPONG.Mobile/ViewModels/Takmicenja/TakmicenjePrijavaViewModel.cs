using FIT_PONG.Mobile.APIServices;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja
{
    class TakmicenjePrijavaViewModel: BaseViewModel
    {
        public TakmicenjeAPIService takmicenjeApiService { get; set; } = new TakmicenjeAPIService();

        public TakmicenjePrijavaViewModel(SharedModels.Takmicenja _takmicenje = null)
        {
            TakmicenjeID = _takmicenje.ID;
            isTim = _takmicenje.Vrsta == "Double";
            listaIgraca = new ObservableCollection<SharedModels.Users>();
            Igrac1 = -1;
            Igrac2 = -1;
        }

        public ObservableCollection<SharedModels.Users> listaIgraca { get; set; }
        public int TakmicenjeID { get; set; }
        public bool isTim  { get; set; }
        private string _naziv;
        public string Naziv { get => _naziv; set => SetProperty(ref _naziv, value); }
        private int _igrac1;
        public int Igrac1 { get => _igrac1; set => SetProperty(ref _igrac1, value); }
        private int _igrac2;
        public int Igrac2 { get => _igrac2; set => SetProperty(ref _igrac2, value); }

        public async void NapuniComboBox()
        {
            var usersService = new UsersAPIService();
            var igraci = await usersService.GetAll<PagedResponse<SharedModels.Users>>();

            do
            {
                foreach (var igrac in igraci.Stavke)
                {
                    listaIgraca.Add(igrac);
                }

                if (igraci.IducaStranica != null)
                {
                    int pozicija = igraci.IducaStranica.ToString().LastIndexOf("/") + 1;
                    string resurs = igraci.IducaStranica.ToString().Substring(pozicija);
                    usersService = new UsersAPIService(resurs);
                    igraci = await usersService.GetAll<PagedResponse<SharedModels.Users>>();
                }
                else
                {
                    igraci = null;
                }

            } while (igraci != null);
        }

        public async Task<Prijave> DodajPrijavu()
        {
            if (Validacija()==false)
                return default(SharedModels.Prijave);

            PrijavaInsert obj = new PrijavaInsert
            {
                Igrac1ID = this.Igrac1,
                Igrac2ID = this.Igrac2,
                isTim = this.isTim,
                Naziv = this.Naziv
            };
            var rezultat = await takmicenjeApiService.Prijava(this.TakmicenjeID, obj);
            return rezultat;
        }

        private bool Validacija()
        {
            var listaErrora = new List<string>();
            if (Igrac1 == -1)
                listaErrora.Add("Morate unijeti igraca za prijavu.");

            if (isTim && Igrac2 == -1)
                listaErrora.Add("Morate unijeti suparnika.");

            if (isTim && String.IsNullOrEmpty(Naziv))
                listaErrora.Add("Naziv je obavezno polje.");

            if (isTim && Naziv.Length > 50)
                listaErrora.Add("Naziv ne smije sadržavati više od 50 karaktera.");
            //var prijave = await takmicenjeApiService.GetPrijave(TakmicenjeID);
            //if (prijave.Where(d => d.Igrac1ID == Igrac1 || d.Igrac2ID == Igrac1).SingleOrDefault() != null || prijave.Where(d => d.Igrac1ID == Igrac2 || d.Igrac2ID == Igrac2).SingleOrDefault() != null)
            //    listaErrora.Add("Vec ste prijavljeni na ovo takmicenje.");

            //if (prijave.Where(d => d.Naziv == Naziv).SingleOrDefault() != null)
            //    listaErrora.Add("Naziv tima je zauzet.");

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
