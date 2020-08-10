using FIT_PONG.Mobile.ViewModels.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace FIT_PONG.Mobile.Views.Users
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UsersStatistike : ContentPage
    {
        UsersStatistikeViewModel viewModel;
        public UsersStatistike(SharedModels.Users _user)
        {
            InitializeComponent();
            BindingContext = viewModel = new UsersStatistikeViewModel(_user);
        }
    }
}