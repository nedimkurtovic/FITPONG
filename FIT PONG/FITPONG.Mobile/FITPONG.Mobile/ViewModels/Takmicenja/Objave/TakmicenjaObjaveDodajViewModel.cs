using FIT_PONG.Mobile.APIServices;
using FIT_PONG.SharedModels;
using FIT_PONG.SharedModels.Requests.Objave;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja.Objave
{
    public class TakmicenjaObjaveDodajViewModel:BaseViewModel
    {
        public TakmicenjaObjaveDodajViewModel(SharedModels.Takmicenja _takmicenje)
        {
            Takmicenje = _takmicenje;
            objaveAPIService = new BaseAPIService($"feeds/{Takmicenje.FeedID}/objave");
        }
        public BaseAPIService objaveAPIService { get; set; }
        public SharedModels.Takmicenja Takmicenje { get; set; }

        private string _naziv;
        public string Naziv { get => _naziv; set => SetProperty(ref _naziv,value); }

        private string _content;
        public string Content { get => _content; set => SetProperty(ref _content,value); }

        public async Task<SharedModels.Objave> DodajNovuObjavu()
        {
            if (!Validacija())
                return default(SharedModels.Objave);

            var request = new ObjaveInsertUpdate
            {
                Naziv = this.Naziv,
                Content = this.Content
            };

            var rezultat = await objaveAPIService.Insert<SharedModels.Objave>(request);
            return rezultat;
        }
        private bool Validacija()
        {
            var listaErrora = new List<string>();
            if (String.IsNullOrEmpty(Naziv) || String.IsNullOrWhiteSpace(Naziv))
                listaErrora.Add("Morate unijeti naziv objave");
            if (String.IsNullOrEmpty(Content) || String.IsNullOrWhiteSpace(Content))
                listaErrora.Add("Morate unijeti sadržaj objave");

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
