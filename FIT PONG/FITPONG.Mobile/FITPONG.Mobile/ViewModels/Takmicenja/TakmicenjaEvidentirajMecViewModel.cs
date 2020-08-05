using FIT_PONG.Mobile.APIServices;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja
{
    public class TakmicenjaEvidentirajMecViewModel:BaseViewModel
    {
        public EvidencijaMeca evidencijaMeca { get; set; }
        public SharedModels.Takmicenja takmicenje { get; set; }


        public TakmicenjaEvidentirajMecViewModel(EvidencijaMeca _evidencijaMeca, SharedModels.Takmicenja _takmicenje)
        {
            evidencijaMeca = _evidencijaMeca;
            takmicenje = _takmicenje;
        }
        private int _rezultattim1;
        public int RezultatTim1 { get => _rezultattim1; set => SetProperty(ref _rezultattim1, value); }

        private int _rezultattim2;

        public int RezultatTim2 { get => _rezultattim2; set => SetProperty(ref _rezultattim2, value); }
        public TakmicenjeAPIService takmicenjeAPIService { get; set; } = new TakmicenjeAPIService();
        public ICommand EvidentirajKomanda { get; set; }

        public async Task<bool> Evidencija()
        {
            if (!Validacija())
                return false;
            evidencijaMeca.RezultatTim1 = RezultatTim1;
            evidencijaMeca.RezultatTim2 = RezultatTim2;
            var rezultat = await takmicenjeAPIService.EvidentirajMec(takmicenje.ID, evidencijaMeca);
            if(rezultat != default(EvidencijaMeca))
            {
                await Application.Current.MainPage.DisplayAlert("Uspjeh", "Uspješno evidentiran meč", "OK");
                return true;
            }
            return false;
        }
        private bool Validacija()
        {
            var listaErrora = new List<string>();
            if (RezultatTim1 < 0 || RezultatTim1 > 5)
                listaErrora.Add($"Unesite ispravno rezultat za {evidencijaMeca.NazivTim1}");
            if (RezultatTim2 < 0 || RezultatTim2 > 5)
                listaErrora.Add($"Unesite ispravno rezultat za {evidencijaMeca.NazivTim2}");
            if (RezultatTim1 == RezultatTim2)
                listaErrora.Add("Meč mora imati pobjednika");

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
