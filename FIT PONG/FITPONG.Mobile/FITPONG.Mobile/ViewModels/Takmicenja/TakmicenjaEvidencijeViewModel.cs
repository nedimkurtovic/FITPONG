using FIT_PONG.Mobile.APIServices;
using FIT_PONG.SharedModels.Requests.Takmicenja;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja
{
    public class TakmicenjaEvidencijeViewModel
    {
        public TakmicenjaEvidencijeViewModel(SharedModels.Takmicenja _takmicenje)
        {
            Takmicenje = _takmicenje;
            ListaEvidencija = new ObservableCollection<EvidencijaMeca>();
            GetEvidencije = new Command(async () => await DobaviEvidencije());
        }
        public TakmicenjeAPIService takmicenjeAPIService { get; set; } = new TakmicenjeAPIService();
        public SharedModels.Takmicenja Takmicenje { get; set; }
        public ObservableCollection<EvidencijaMeca> ListaEvidencija { get; set; }
        public ICommand GetEvidencije { get; set; }
        async Task DobaviEvidencije()
        {
            var rezultat = await takmicenjeAPIService.GetEvidencije(Takmicenje.ID);
            foreach (var i in rezultat)
                ListaEvidencija.Add(i);
        }
    }
}
