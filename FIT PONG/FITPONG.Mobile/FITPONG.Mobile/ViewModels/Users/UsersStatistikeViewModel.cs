using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;

namespace FIT_PONG.Mobile.ViewModels.Users
{
    public class UsersStatistikeViewModel:BaseViewModel
    {
        public UsersStatistikeViewModel(SharedModels.Users _user)
        {
            ListaStatistika = new ObservableCollection<SharedModels.Statistike>();
            foreach(var i in _user.statistike)
            {
                ListaStatistika.Add(i);
            }
        }

        public ObservableCollection<SharedModels.Statistike> ListaStatistika { get; set; }
    }
}
