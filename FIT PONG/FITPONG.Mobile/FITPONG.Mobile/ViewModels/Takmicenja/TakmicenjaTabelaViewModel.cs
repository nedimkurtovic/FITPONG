using FIT_PONG.Mobile.APIServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja
{
    public class TakmicenjaTabelaViewModel:BaseViewModel
    {
        public TakmicenjaTabelaViewModel(SharedModels.Takmicenja _takmicenje)
        {
            Takmicenje = _takmicenje;
            TabelaStavke = new ObservableCollection<SharedModels.Requests.Takmicenja.TabelaStavka>();
            DobaviTabelu = new Command(async () => await GetTabela());
        }
        public SharedModels.Takmicenja Takmicenje { get; set; }
        public TakmicenjeAPIService takmicenjeAPIService { get; set; } = new TakmicenjeAPIService();

        public ObservableCollection<SharedModels.Requests.Takmicenja.TabelaStavka> TabelaStavke { get; set; }
        public ICommand DobaviTabelu { get; set; }
        public async Task GetTabela()
        {
            var rezultat = await takmicenjeAPIService.GetTabela(Takmicenje.ID);
            foreach (var i in rezultat)
                TabelaStavke.Add(i);
        }

    }
}
