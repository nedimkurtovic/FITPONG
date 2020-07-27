using FIT_PONG.Mobile.ViewModels.Users;
using Org.BouncyCastle.Crypto.Tls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [DesignTimeVisible(false)]
    public partial class UsersDetalji : ContentPage
    {
        UsersDetaljiViewModel viewModel;
        public UsersDetalji(UsersDetaljiViewModel model)
        {
            InitializeComponent();
            BindingContext = viewModel = model;
        }
        public UsersDetalji()
        {
            InitializeComponent();

            var user = new SharedModels.Users
            {
                PrikaznoIme = "Item 1",
                ELO = 1000
            };

            viewModel = new UsersDetaljiViewModel(user);
            BindingContext = viewModel;
        }
    }
}