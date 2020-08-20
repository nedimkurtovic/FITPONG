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

        public UsersPrijaveViewModel(SharedModels.Users user)
        {
            DodajPrijave(user.listaPrijava);
        }

        public ObservableCollection<Prijave> listaPrijava { get; set; } = new ObservableCollection<Prijave>();
        
        private void DodajPrijave(List<Prijave> prijave)
        {
            foreach (var item in prijave)
                listaPrijava.Add(item);
        }

        public async Task<Prijave> OtkaziPrijavu(int id)
        {
            var p = listaPrijava.Where(d => d.ID == id).Single();
            //if (p.Takmicenje.Inicirano ?? true)
            //{
            //    await Application.Current.MainPage.DisplayAlert("Greska", "Ne mozete otkazati takmicenje koje je vec inicirano.", "OK");
            //    return default(SharedModels.Prijave);
            //}

            var rezultat = await takmicenjeApiService.OtkaziPrijavu(id);
            listaPrijava.Remove(p);

            return rezultat;
        }

    }
}
