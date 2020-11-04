using FIT_PONG.Database.DTOs;
using FIT_PONG.Mobile.APIServices;
using FIT_PONG.SharedModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Users
{
    class UsersPrijaveViewModel: BaseViewModel
    {
        public TakmicenjeAPIService takmicenjeApiService { get; set; } = new TakmicenjeAPIService();

        public bool IsVlasnik { get; set; }
        public UsersPrijaveViewModel(SharedModels.Users user)
        {
            DodajPrijave(user.listaPrijava);
            IsVlasnik = user.ID == BaseAPIService.ID;
        }

        public ObservableCollection<Prijave> listaPrijava { get; set; } = new ObservableCollection<Prijave>();
        
        private void DodajPrijave(List<Prijave> prijave)
        {
            foreach (var item in prijave)
                listaPrijava.Add(item);
        }

        public async Task<Prijave> OtkaziPrijavu(int id)
        {
            if (!Validacija())
                return default(Prijave);
            var p = listaPrijava.Where(d => d.ID == id).Single();
         
            var rezultat = await takmicenjeApiService.OtkaziPrijavu(id);
            if(rezultat != default(Prijave))
                listaPrijava.Remove(p);

            return rezultat;
        }
        private bool Validacija()
        {
            return IsVlasnik;
        }

    }
}
