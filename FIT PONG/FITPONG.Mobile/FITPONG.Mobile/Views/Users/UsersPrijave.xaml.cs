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
    public partial class UsersPrijave : ContentPage
    {
        UsersPrijaveViewModel viewModel;
        public UsersPrijave(SharedModels.Users user)
        {
            BindingContext = viewModel = new UsersPrijaveViewModel(user);
            InitializeComponent();
            lblEmpty.IsVisible = viewModel.listaPrijava.Count == 0;
        }

        private async void btnOtkazi_Clicked(object sender, EventArgs e)
        {
            var x = ((Button)sender).BindingContext;

            var rezultat = await viewModel.OtkaziPrijavu(int.Parse(x.ToString()));

            if(rezultat == default(SharedModels.Prijave))
            {
                await Application.Current.MainPage.DisplayAlert("Greska", "Greska prilikom otkazivanja prijave.", "OK");
            }
        }
    }
}