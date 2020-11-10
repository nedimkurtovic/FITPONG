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
    public partial class PotvrdiMejlPassword : ContentPage
    {
        PotvrdiMejlPasswordViewModel viewModel;

        public PotvrdiMejlPassword(int userId,string tip, string email)
        {
            BindingContext = viewModel = new PotvrdiMejlPasswordViewModel(userId, tip, email);
            viewModel.Navigacija = Navigation;
            InitializeComponent();
        }
    }
}