using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.ViewModels;
using FIT_PONG.SharedModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics.Contracts;
using System.Drawing;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace FIT_PONG.Mobile.ViewModels.Users
{
    public class UsersDetaljiViewModel:BaseViewModel
    {
        public UsersAPIService _UsersAPIService { get; set; }
        public ObservableCollection<SharedModels.Users> ListaPreporucenihKorisnika { get; set; }
        public SharedModels.Users User { get; set; }
        public int BrojPobjeda { get; set; }
        public int BrojPoraza { get; set; }
        public bool isEditVisible { get; set; }
        public UsersDetaljiViewModel(SharedModels.Users _user = null)
        {
            _UsersAPIService = new UsersAPIService();
            ListaPreporucenihKorisnika = new ObservableCollection<SharedModels.Users>();
            Title = _user?.PrikaznoIme;
            User = _user;
            BrojPobjeda = GetBrojPobjeda();
            BrojPoraza = GetBrojPoraza();
            GetPreporuceneCommand = new Command(async () => await UnesiPreporucene());
            isEditVisible = _user.ID == BaseAPIService.ID;
        }
        public UsersDetaljiViewModel()
        {

        }
        public Command  GetPreporuceneCommand  { get; set; }
        private int GetBrojPoraza()
        {
            var suma = 0;
            foreach (var i in User.statistike)
            {
                suma += (i.BrojOdigranihMeceva - i.BrojSinglePobjeda);
                suma += (i.BrojOdigranihMeceva -  i.BrojTimskihPobjeda);
            }
            return suma;
        }

        private int GetBrojPobjeda()
        {
            var suma = 0;
            foreach(var i in User.statistike)
            {
                suma += i.BrojSinglePobjeda + i.BrojTimskihPobjeda;
            }
            return suma;
        }

        async Task UnesiPreporucene()
        {
            var rezultat = await _UsersAPIService.GetRecommended(User.ID);
            Device.BeginInvokeOnMainThread(() =>
            {
                foreach (var i in rezultat)
                    ListaPreporucenihKorisnika.Add(i);
            });
           
        }
        public bool Vlasnik()
        {
            if (User != null)
                return User.ID == BaseAPIService.ID;
            return false;          
        }
    }
}
