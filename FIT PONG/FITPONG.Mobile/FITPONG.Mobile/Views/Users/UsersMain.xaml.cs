using FIT_PONG.Mobile.APIServices;
using FIT_PONG.Mobile.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersMain : TabbedPage
    {
        BaseAPIService apiServis = new BaseAPIService("users");
        public UsersMain(SharedModels.Users _user)
        {
            InitializeComponent();

            Title = _user.PrikaznoIme;

            var detaljiStranica = new UsersDetalji(new UsersDetaljiViewModel(_user));
            detaljiStranica.Title = "Detalji";
            Children.Add(detaljiStranica);

            var statistikeStranica = new UsersStatistike(_user);
            statistikeStranica.Title = "Statistike";
            Children.Add(statistikeStranica);

            var prijaveStranica = new UsersPrijave();
            prijaveStranica.Title = "Prijave";
            Children.Add(prijaveStranica);
        }
    }
}