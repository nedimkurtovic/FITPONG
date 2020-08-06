using FIT_PONG.Mobile.APIServices;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests;
using FIT_PONG.SharedModels.Requests.Objave;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja.Objave
{
    public class TakmicenjaObjaveViewModel:BaseViewModel
    {
        public ObservableCollection<SharedModels.Objave> ListaObjava { get; set; }
        public TakmicenjaObjaveViewModel(SharedModels.Takmicenja _takmicenje)
        {
            ListaObjava = new ObservableCollection<SharedModels.Objave>();
            Takmicenje = _takmicenje;
            objaveAPIService = new BaseAPIService($"feeds/{_takmicenje.FeedID}/objave");
            iducaStranica = "";
            DobaviObjaveKomanda = new Command(async () => await dobaviObjaveFunkcija());
            DobaviJosObjavaKomanda = new Command(async () => await dobaviJosObjavaFunkcija());
            btnDobaviJosVisible = false;
        }
        public BaseAPIService objaveAPIService { get; set; }
        public SharedModels.Takmicenja Takmicenje { get; set; }

        private string _naziv;
        public string Naziv { get=>_naziv; set => SetProperty(ref _naziv,value); }

        private string iducaStranica;
        public ICommand DobaviObjaveKomanda { get; set; }
        public ICommand DobaviJosObjavaKomanda { get; set; }

        public bool Vlasnik()
        {
            return BaseAPIService.ID == Takmicenje.KreatorID;
        }
        public bool btnDobaviJosVisible { get; set; }
        async Task dobaviObjaveFunkcija()
        {
            ListaObjava.Clear();
            var searchReq = new ObjaveSearch
            {
                Naziv = this.Naziv
            };
            var rezultat = await objaveAPIService.GetAll<PagedResponse<SharedModels.Objave>>(searchReq);
            if(rezultat != default(PagedResponse<SharedModels.Objave>))
                dodajUListu(rezultat);
        }
        async Task dobaviJosObjavaFunkcija()
        {
            var rezultat = await objaveAPIService.GetAllPaged<PagedResponse<SharedModels.Objave>>(iducaStranica);
            if (rezultat != default(PagedResponse<SharedModels.Objave>))
                dodajUListu(rezultat);
        }
        private void dodajUListu(PagedResponse<SharedModels.Objave> rez)
        {
            foreach (var i in rez.Stavke)
                ListaObjava.Add(i);
            iducaStranica = rez.IducaStranica?.ToString();
            btnDobaviJosVisible = iducaStranica == null ? false : true;
        }
    }
}
