using FIT_PONG.Mobile.APIServices;
using FIT_PONG.SharedModels.Requests.Objave;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja.Objave
{
    public class TakmicenjaObjaveEditViewModel:BaseViewModel
    {
        public TakmicenjaObjaveEditViewModel(SharedModels.Objave _objava)
        {
            objaveAPIService = new BaseAPIService("objave");
            Objava = _objava;
            Naziv = Objava.Naziv;
            Content = Objava.Content;
        }
        public BaseAPIService objaveAPIService { get; set; }
        public SharedModels.Takmicenja Takmicenje { get; set; }

        private string _naziv;
        public string Naziv { get => _naziv; set => SetProperty(ref _naziv, value); }

        private string _content;
        public string Content { get => _content; set => SetProperty(ref _content, value); }
        public SharedModels.Objave Objava { get; set; }

        public async Task<SharedModels.Objave> UpdateObjavu()
        {
            if (!Validacija())
                return default(SharedModels.Objave);

            var request = new ObjaveInsertUpdate
            {
                Naziv = this.Naziv,
                Content = this.Content
            };

            var rezultat = await objaveAPIService.Update<SharedModels.Objave>(Objava.ID, request,"PUT");
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
