using FIT_PONG.Mobile.ViewModels;
using FIT_PONG.SharedModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace FIT_PONG.Mobile.ViewModels.Users
{
    public class UsersDetaljiViewModel:BaseViewModel
    {
        public SharedModels.Users User { get; set; }
        public int BrojPobjeda { get; set; }
        public int BrojPoraza { get; set; }
        public UsersDetaljiViewModel(SharedModels.Users _user = null)
        {
            Title = _user?.PrikaznoIme;
            User = _user;
            BrojPobjeda = GetBrojPobjeda();
            BrojPoraza = GetBrojPoraza();
        }
        public UsersDetaljiViewModel()
        {

        }
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
    }
}
