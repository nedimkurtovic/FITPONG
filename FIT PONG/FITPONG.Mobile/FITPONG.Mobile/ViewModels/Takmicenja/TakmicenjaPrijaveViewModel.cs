using FIT_PONG.Database.DTOs;
using FIT_PONG.Mobile.APIServices;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FIT_PONG.Mobile.ViewModels.Takmicenja
{
    public class TakmicenjaPrijaveViewModel:BaseViewModel
    {
        private readonly SharedModels.Takmicenja Takmicenje;

        public ObservableCollection<Prijava> ListaPrijava { get; set; } = new ObservableCollection<Prijava>();
        public TakmicenjeAPIService TakmicenjeApiService { get; set; } = new TakmicenjeAPIService();
        public bool prijaveVisible { get; set; }
        public TakmicenjaPrijaveViewModel(SharedModels.Takmicenja _takmicenje)
        {
            Takmicenje = _takmicenje;
            //prijaveVisible = _takmicenje.Inicirano == false && Vlasnik(); ovo je rijeseno u takmicenje main
            DodajPrijave(_takmicenje.Prijave);
        }

        private void DodajPrijave(List<Prijava> prijave)
        {
            foreach (var item in prijave)
                ListaPrijava.Add(item);
        }
        public bool Vlasnik()
        {
            return BaseAPIService.ID == Takmicenje.KreatorID;
        }
        public async Task<SharedModels.Prijave> BlokirajPrijavu(int id)
        {
            var rezultat = await TakmicenjeApiService.BlokirajPrijavu(Takmicenje.ID, id);
            var p = ListaPrijava.Where(d => d.ID == id).Single();

            ListaPrijava.Remove(p);

            return rezultat;
        }
    }
}
